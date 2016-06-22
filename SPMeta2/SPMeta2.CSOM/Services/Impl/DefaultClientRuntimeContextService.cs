using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Microsoft.SharePoint.Client;
using System.Threading;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.Services.Impl
{
    public class DefaultClientRuntimeContextService : ClientRuntimeContextServiceBase
    {
        #region constructors

        public DefaultClientRuntimeContextService()
        {
            // changes as per Microsoft recommendations
            // https://msdn.microsoft.com/en-us/library/office/dn889829.aspx?f=255&MSPPError=-2147217396#BKMK_Bestpracticestohandlethrottling
            // https://github.com/SubPointSolutions/spmeta2/issues/849
            // https://www.yammer.com/spmeta2feedback/#/threads/show?threadId=725945901&messageId=725945901

            // 5 re-try, 30 sec each and x2 backoff multiplier 

            ExecuteQueryDelayInMilliseconds = 30000;

            ExecuteQueryRetryAttempts = 5;
            ExecuteQueryRetryDelayMultiplier = 2;

            InitAllowedStatusCodes();
            InitAllowedIISResetSocketStatusCodes();
        }

        private void InitAllowedIISResetSocketStatusCodes()
        {
            _allowedIISResetSocketStatusCodes.Add(SocketError.ConnectionReset);
        }

        private void InitAllowedStatusCodes()
        {
            _allowedStatusCodes.Add(429);
            _allowedStatusCodes.Add(503);
        }

        #endregion

        #region properties

        public int ExecuteQueryDelayInMilliseconds { get; set; }
        public int ExecuteQueryRetryAttempts { get; set; }
        public double ExecuteQueryRetryDelayMultiplier { get; set; }

        private readonly List<int> _allowedStatusCodes = new List<int>();

        protected virtual List<int> AllowedStatusCodes
        {
            get { return _allowedStatusCodes; }
        }

        private readonly List<SocketError> _allowedIISResetSocketStatusCodes = new List<SocketError>();

        protected virtual List<SocketError> AllowedIISResetSocketStatusCodes
        {
            get { return _allowedIISResetSocketStatusCodes; }
        }

        public Action<ClientRuntimeContext> CustomExecuteQueryHandler { get; set; }

        #endregion

        #region methods

        protected virtual void InternalExecuteQuery(ClientRuntimeContext context)
        {
            if (CustomExecuteQueryHandler != null)
            {
                CustomExecuteQueryHandler(context);
            }
            else
            {
                context.ExecuteQuery();
            }
        }

        public override void ExecuteQuery(ClientRuntimeContext context)
        {
            var currentRetryCount = 0;
            var currentRetryDelayInMilliseconds = ExecuteQueryDelayInMilliseconds;

            Exception lastException = null;

            while (currentRetryCount < ExecuteQueryRetryAttempts)
            {
                try
                {
                    InternalExecuteQuery(context);
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;

                    if (ShouldRetryExecuteQuery(ex))
                    {
                        currentRetryCount++;
                        currentRetryDelayInMilliseconds = GetNextExecuteQueryDelayInMilliseconds(currentRetryDelayInMilliseconds);

                        Thread.Sleep(currentRetryDelayInMilliseconds);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var message = string.Format("ClientRuntimeContext.ExecuteQuery() exceeded [{0}] retry attempts. Check InnerException for the last returned exception.", ExecuteQueryRetryAttempts);
            throw new SPMeta2Exception(message, lastException);
        }

        protected virtual int GetNextExecuteQueryDelayInMilliseconds(int currentDelay)
        {
            return (int)(currentDelay * ExecuteQueryRetryDelayMultiplier);
        }

        protected virtual bool ShouldRetryExecuteQuery(Exception ex)
        {
            // O365 related handling
            if (IsAllowedHttpWebResponseStatusCode(ex))
                return true;

            if (IsAllowedConnectionResponseStatuses(ex))
                return true;

            // local IISReset or something else?
            if (IsAllowedWebException(ex))
                return true;

            // The request uses too many resources ?
            if (IsRequestUsesTooManyResources(ex))
                return true;

            // some weird exception with sql or something
            if (IsWeirdSQLException(ex))
                return true;

            return false;
        }

        protected virtual bool IsWeirdSQLException(Exception ex)
        {
            // happens randomly, while adding files or creating pages
            // something weird with the stream or whatever
            // https://github.com/SubPointSolutions/spmeta2/issues/567

            return ex != null
                   && ex.ToString().Contains("0x80131904");
        }

        protected virtual bool IsRequestUsesTooManyResources(Exception ex)
        {
            // happens while activating publishing site features
            // or creaeting publishing site via CSOM
            // well-known stuff
            // https://github.com/SubPointSolutions/spmeta2/issues/567

            return ex != null
                   && !string.IsNullOrEmpty(ex.Message)
                   && ex.Message.ToLower().Contains("the request uses too many resources");
        }



        protected virtual bool IsAllowedWebException(Exception ex)
        {
            // handles allowed WebException
            // IISReset, for instance

            var webEx = ex as WebException;

            if (webEx != null)
            {
                if (IsIISReset(webEx))
                    return true;
            }

            return false;
        }

        protected virtual bool IsIISReset(WebException ex)
        {
            if (ex != null && ex.InnerException != null)
            {
                var lastInnerException = ex.InnerException;

                while (lastInnerException.InnerException != null)
                    lastInnerException = lastInnerException.InnerException;

                if (lastInnerException is SocketException)
                {
                    var socketException = lastInnerException as SocketException;
                    return AllowedIISResetSocketStatusCodes.Contains(socketException.SocketErrorCode);
                }
            }

            return false;
        }

        protected virtual bool IsAllowedHttpWebResponseStatusCode(Exception ex)
        {
            // handles allowed http responce statuses with AllowedStatusCodes collection
            var webEx = ex as WebException;

            if (webEx != null && webEx.Response != null)
            {
                var webHttpResponce = webEx.Response as HttpWebResponse;

                if (webHttpResponce != null)
                {
                    return AllowedStatusCodes.Contains((int)webHttpResponce.StatusCode);
                }
            }

            return false;
        }

        private bool IsAllowedConnectionResponseStatuses(Exception ex)
        {
            // droped network, wi-fi or proxy
            // comes with 'System.Net.WebException: The underlying connection was closed: The connection was closed unexpectedly.'
            var webEx = ex as WebException;

            if (webEx != null)
            {
                if (webEx.Status == WebExceptionStatus.ConnectionClosed || webEx.Status == WebExceptionStatus.ConnectFailure)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
