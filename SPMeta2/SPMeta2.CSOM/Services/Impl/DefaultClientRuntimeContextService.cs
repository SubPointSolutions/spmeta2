using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        #endregion

        #region methods

        public override void ExecuteQuery(ClientRuntimeContext context)
        {
            var currentRetryCount = 0;
            var currentRetryDelayInMilliseconds = ExecuteQueryDelayInMilliseconds;

            while (currentRetryCount < ExecuteQueryRetryAttempts)
            {
                try
                {
                    context.ExecuteQuery();
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

            throw new SPMeta2Exception(string.Format("ClientRuntimeContext.ExecuteQuery() exceeded [{0}] retry attempts.", ExecuteQueryDelayInMilliseconds));
        }

        protected virtual int GetNextExecuteQueryDelayInMilliseconds(int currentDelay)
        {
            return (int)(currentDelay * 1.5);
        }

        protected virtual bool ShouldRetryExecuteQuery(Exception ex)
        {
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
