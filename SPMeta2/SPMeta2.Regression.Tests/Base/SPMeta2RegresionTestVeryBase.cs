using SPMeta2.Services;
using SPMeta2.Services.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Tests.Base
{
    public class SPMeta2RegresionTestVeryBase
    {
        static SPMeta2RegresionTestVeryBase()
        {
            ServiceContainer.Instance.ReplaceService(
                                typeof(TraceServiceBase),
                                new TraceableTraceService());
        }
    }


    public class TraceableTraceService : TraceSourceService
    {
        protected override void TraceEvent(int id, System.Diagnostics.TraceEventType messageType, object message, Exception exception)
        {
            base.TraceEvent(id, messageType, message, exception);

            var traceString = GetTraceEventString(id, messageType, message, exception);

            Trace.WriteLine(traceString);
        }
    }
}
