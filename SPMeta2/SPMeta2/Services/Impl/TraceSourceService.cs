using System;
using System.Diagnostics;
using SPMeta2.Utils;

namespace SPMeta2.Services.Impl
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
            TraceSource = new TraceSource(traceSourceName);
        }

        #endregion

        #region properties

        public TraceSource TraceSource;

        public override bool IsCriticalEnabled
        {
            get
            {
                return ((TraceSource.Switch.Level & SourceLevels.Critical) == SourceLevels.Critical);
            }
            set
            {

            }
        }

        public override bool IsErrorEnabled
        {
            get
            {
                return ((TraceSource.Switch.Level & SourceLevels.Error) == SourceLevels.Error);
            }
            set
            {

            }
        }

        public override bool IsInformationEnabled
        {
            get
            {
                return ((TraceSource.Switch.Level & SourceLevels.Information) == SourceLevels.Information);
            }
            set
            {

            }
        }

        public override bool IsVerboseEnabled
        {
            get
            {
                return ((TraceSource.Switch.Level & SourceLevels.Verbose) == SourceLevels.Verbose);
            }
            set
            {

            }
        }

        public override bool IsWarningEnabled
        {
            get
            {
                return ((TraceSource.Switch.Level & SourceLevels.Warning) == SourceLevels.Warning);
            }
            set
            {

            }
        }

        #endregion

        #region methods

        public override void Critical(int id, object message, Exception exception)
        {
            if (IsCriticalEnabled)
            {
                TraceEvent(id, TraceEventType.Critical, message, exception);
            }
        }

        public override void Error(int id, object message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                TraceEvent(id, TraceEventType.Error, message, exception);
            }
        }

        public override void Warning(int id, object message, Exception exception)
        {
            if (IsWarningEnabled)
            {
                TraceEvent(id, TraceEventType.Warning, message, exception);
            }
        }

        public override void Information(int id, object message, Exception exception)
        {
            if (IsInformationEnabled)
            {
                TraceEvent(id, TraceEventType.Information, message, exception);
            }
        }

        public override void Verbose(int id, object message, Exception exception)
        {
            if (IsVerboseEnabled)
            {
                TraceEvent(id, TraceEventType.Verbose, message, exception);
            }
        }

        #endregion

        #region utils

        protected virtual string GetTraceEventString(int id, TraceEventType messageType, object message, Exception exception)
        {
            var traceString = string.Empty;
            var messageString = message == null ? string.Empty : message.ToString();

            if (exception != null)
            {
                var subMessage = messageString;

                if (!subMessage.EndsWith("."))
                    subMessage += ".";

                traceString = string.Format("{0} Exception: [{1}]", subMessage, exception);
            }
            else
            {
                var subMessage = messageString;

                if (!subMessage.EndsWith("."))
                    subMessage += ".";

                traceString = string.Format("{0}", subMessage);
            }

            return traceString;
        }

        protected virtual void TraceEvent(int id, TraceEventType messageType, object message, Exception exception)
        {
            var traceString = GetTraceEventString(id, messageType, message, exception);

            TraceSource.TraceEvent(messageType, id, traceString);

            if (AutoFlush)
            {
                TraceSource.Flush();
            }
        }

        #endregion

        public override void Flush()
        {
            TraceSource.Flush();
        }

        public override void TraceActivityStart(int id, object message)
        {
            TraceSource.TraceEvent(TraceEventType.Start, id, ConvertUtils.ToString(message));
        }

        public override void TraceActivityStop(int id, object message)
        {
            TraceSource.TraceEvent(TraceEventType.Stop, id, ConvertUtils.ToString(message));
        }

        public override void TraceActivityTransfer(int id, object message, Guid relatedActivityId)
        {
            TraceSource.TraceTransfer(id, ConvertUtils.ToString(message), relatedActivityId);
        }

        public override Guid CurrentActivityId
        {
            get
            {
                return Trace.CorrelationManager.ActivityId;
            }
            set { Trace.CorrelationManager.ActivityId = value; }
        }
    }
}
