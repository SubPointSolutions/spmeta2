using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Services.Impl;
using SPMeta2.Services;
using SPMeta2.Services.Impl;

namespace SPMeta2.CSOM.Extensions
{
    public static class ClientRuntimeContextExtensions
    {
        #region static

        static ClientRuntimeContextExtensions()
        {
            DefaultClientRuntimeContextService = new DefaultClientRuntimeContextService();
            DefaultTraceServiceBase = new TraceSourceService();
        }

        #endregion

        #region properties

        private static readonly ClientRuntimeContextServiceBase DefaultClientRuntimeContextService;
        private static readonly TraceServiceBase DefaultTraceServiceBase;

        private static TraceServiceBase TraceService
        {
            get { return ServiceContainer.Instance.GetService<TraceServiceBase>() ?? DefaultTraceServiceBase; }
        }

        private static ClientRuntimeContextServiceBase ClientRuntimeContextService
        {
            get
            {
                return ServiceContainer.Instance.GetService<ClientRuntimeContextServiceBase>() ?? DefaultClientRuntimeContextService;
            }
        }

        #endregion

        #region methods

        public static bool IsSharePointOnlineContext(this  ClientRuntimeContext context)
        {
            return ClientRuntimeContextService.IsSharePointOnlineContext(context);
        }

        public static void ExecuteQueryWithTrace(this  ClientRuntimeContext context)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");

            // delegating to ClientRuntimeContextService instance
            // Implement fault tolerant provision for CSOM #567
            // https://github.com/SubPointSolutions/spmeta2/issues/567
            //context.ExecuteQuery();

            ClientRuntimeContextService.ExecuteQuery(context);
        }

        #endregion
    }
}
