using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Services;

namespace SPMeta2.CSOM.Extensions
{
    public static class ClientRuntimeContextExtensions
    {
        #region static

        static ClientRuntimeContextExtensions()
        {
            TraceService = ServiceContainer.Instance.GetService<TraceServiceBase>();
        }

        #endregion

        #region properties

        private static TraceServiceBase TraceService { get; set; }

        #endregion

        #region methods

        public static void ExecuteQueryWithTrace(this  ClientRuntimeContext context)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");
            context.ExecuteQuery();
        }

        #endregion
    }
}
