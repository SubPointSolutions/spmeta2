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
            ExecuteQueryDelayInMilliseconds = 1000;
            ExecuteQueryRetryAttempts = 10;

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

            while (currentRetryCount < ExecuteQueryRetryAttempts)
            {
                try
                {
                    InternalExecuteQuery(context);
                    return;
                }
                catch (Exception ex)
                {
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

            throw new SPMeta2Exception(string.Format("ClientRuntimeContext.ExecuteQuery() exceeded [{0}] retry attempts.", ExecuteQueryRetryAttempts));
        }

        protected virtual int GetNextExecuteQueryDelayInMilliseconds(int currentDelay)
        {
            return (int)(currentDelay * 1.5);
        }

        protected virtual bool ShouldRetryExecuteQuery(Exception ex)
        {
            // O365 related handling
            if (IsAllowedHttpWebResponseStatusCode(ex))
                return true;

            // local IISReset or something else?
            if (IsAllowedWebException(ex))
                return true;

            return false;
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

        #endregion
    }
}
