using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Services
{
    public class TraceSourceService : TraceServiceBase
    {
        #region properties

        public TraceSourceService()
            : this("SPMeta2")
        {
        }

        public TraceSourceService(string traceSourceName)
        {
            Trace = new TraceSource("SPMeta2");
        }

        #endregion

        #region properties

        public TraceSource Trace = new TraceSource("SPMeta2");

        #endregion

        #region methods


        public override void Critical(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Critical, message, exception);
        }

        public override void Error(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Error, message, exception);
        }

        public override void Warning(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Warning, message, exception);
        }

        public override void Information(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Information, message, exception);
        }

        public override void Verbose(int id, object message, Exception exception)
        {
            TraceEvent(id, TraceEventType.Verbose, message, exception);
        }

        #endregion

        #region utils

        protected virtual void TraceEvent(int id, TraceEventType messageType, object message, Exception exception)
        {
            var traceString = string.Empty;
            var messageString = message == null ? string.Empty : message.ToString();

            if (exception != null)
            {
                traceString = string.Format("{0}. Exception: [{1}]", messageString, exception);
            }
            else
            {
                traceString = string.Format("{0}.", messageString);
            }

            Trace.TraceEvent(messageType, id, traceString);
            Trace.Flush();
        }

        #endregion
    }
}
