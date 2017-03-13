using System;
using System.Diagnostics;

namespace SPMeta2.Services
{
    public abstract class TraceServiceBase
    {
        #region constructors

        public TraceServiceBase()
        {
            // backward compatibility, always enabled but false for verbose
            // inherited classes should override these returning true-false as per setup

            // some of the extension methods AND actual methods of the trace service implementation use
            // use these flags to avoid String.Format() calculations before even hitting .Log() methods

            // that is a trade off due to performance implications over .ToString() calls on ModelNodes/DefinitionBase
            // such classes use .ToString() override with reflection which is extremely slow

            IsCriticalEnabled = true;
            IsErrorEnabled = true;
            IsWarningEnabled = true;
            IsInformationEnabled = true;
            IsVerboseEnabled = false;

            AutoFlush = true;
        }

        #endregion

        #region properties

        public virtual bool IsCriticalEnabled { get; set; }
        public virtual bool IsErrorEnabled { get; set; }

        public virtual bool IsWarningEnabled { get; set; }
        public virtual bool IsInformationEnabled { get; set; }

        public virtual bool IsVerboseEnabled { get; set; }

        public bool AutoFlush { get; set; }

        #endregion

        #region methods

        public virtual void Flush()
        {

        }

        public virtual void Critical(int id, object message)
        {
            Critical(id, message, null);
        }

        public abstract void Critical(int id, object message, Exception exception);

        public virtual void Error(int id, object message)
        {
            Error(id, message, null);
        }

        public abstract void Error(int id, object message, Exception exception);

        public virtual void Warning(int id, object message)
        {
            Warning(id, message, null);
        }

        public abstract void Warning(int id, object message, Exception exception);

        public virtual void Information(int id, object message)
        {
            Information(id, message, null);
        }

        public abstract void Information(int id, object message, Exception exception);

        public virtual void Verbose(int id, object message)
        {
            Verbose(id, message, null);
        }

        public abstract void Verbose(int id, object message, Exception exception);

        public abstract void TraceActivityStart(int id, object message);
        public abstract void TraceActivityStop(int id, object message);
        public abstract void TraceActivityTransfer(int id, object message, Guid relatedActivityId);

        public abstract Guid CurrentActivityId
        {
            get;
            set;
        }

        #endregion
    }

    public static class TraceServiceBaseExtensions
    {
        #region information

        public static void InformationFormat(this TraceServiceBase traceService, int id, object message, object parameter)
        {
            if (traceService != null && traceService.IsInformationEnabled)
            {
                InformationFormat(traceService, id, message, new object[] { parameter });
            }
        }

        public static void InformationFormat(this TraceServiceBase traceService, int id, object message, object[] parameters)
        {
            if (traceService != null && traceService.IsInformationEnabled)
            {
                InformationFormat(traceService, id, message, parameters, null);
            }
        }

        public static void InformationFormat(this TraceServiceBase traceService, int id, object message, object[] parameters, Exception exception)
        {
            if (traceService != null && traceService.IsInformationEnabled)
            {
                if (message is string)
                {
                    traceService.Information(id, string.Format(message as string, parameters), exception);
                }
                else
                {
                    traceService.Information(id, message, exception);
                }
            }
        }

        #endregion

        #region warning

        public static void WarningFormat(this TraceServiceBase traceService, int id, object message, object parameter)
        {
            if (traceService != null && traceService.IsWarningEnabled)
            {
                WarningFormat(traceService, id, message, new object[] { parameter });
            }
        }

        public static void WarningFormat(this TraceServiceBase traceService, int id, object message, object[] parameters)
        {
            if (traceService != null && traceService.IsWarningEnabled)
            {
                WarningFormat(traceService, id, message, parameters, null);
            }
        }

        public static void WarningFormat(this TraceServiceBase traceService, int id, object message, object[] parameters, Exception exception)
        {
            if (traceService != null && traceService.IsWarningEnabled)
            {
                if (message is string)
                {
                    traceService.Warning(id, string.Format(message as string, parameters), exception);
                }
                else
                {
                    traceService.Warning(id, message, exception);
                }
            }
        }

        #endregion

        #region verbose

        public static void VerboseFormat(this TraceServiceBase traceService, int id, object message, object parameter)
        {
            if (traceService != null && traceService.IsVerboseEnabled)
            {
                if (parameter == null)
                    VerboseFormat(traceService, id, message, new object[] { });
                else
                    VerboseFormat(traceService, id, message, new object[] { parameter });
            }
        }

        public static void VerboseFormat(this TraceServiceBase traceService, int id, object message, object[] parameters)
        {
            if (traceService != null && traceService.IsVerboseEnabled)
            {
                if (parameters == null)
                    VerboseFormat(traceService, id, message, new object[] { }, null);
                else
                    VerboseFormat(traceService, id, message, parameters, null);
            }
        }

        public static void VerboseFormat(this TraceServiceBase traceService, int id, object message, object[] parameters, Exception exception)
        {
            if (traceService != null && traceService.IsVerboseEnabled)
            {
                if (message is string && parameters != null && parameters.Length > 0)
                {
                    traceService.Verbose(id, string.Format(message as string, parameters), exception);
                }
                else
                {
                    traceService.Verbose(id, message, exception);
                }
            }
        }

        #endregion

        #region error

        public static void ErrorFormat(this TraceServiceBase traceService, int id, object message, object parameter)
        {
            if (traceService != null && traceService.IsErrorEnabled)
            {
                if (parameter == null)
                    ErrorFormat(traceService, id, message, new object[] { });
                else
                    ErrorFormat(traceService, id, message, new object[] { parameter });
            }
        }

        public static void ErrorFormat(this TraceServiceBase traceService, int id, object message, object[] parameters)
        {
            if (traceService != null && traceService.IsErrorEnabled)
            {
                if (parameters == null)
                    ErrorFormat(traceService, id, message, new object[] { }, null);
                else
                    ErrorFormat(traceService, id, message, parameters, null);
            }
        }

        public static void ErrorFormat(this TraceServiceBase traceService, int id, object message, object[] parameters, Exception exception)
        {
            if (traceService != null && traceService.IsErrorEnabled)
            {
                if (message is string && parameters != null && parameters.Length > 0)
                {
                    traceService.Error(id, string.Format(message as string, parameters), exception);
                }
                else
                {
                    traceService.Error(id, message, exception);
                }
            }
        }

        #endregion
    }

    public class TraceMethodActivityScope : TraceActivityScope
    {
        public TraceMethodActivityScope(TraceServiceBase traceService, int eventId)
            : base(traceService, eventId, new StackFrame(1).GetMethod().Name)
        {

        }
    }

    public class TraceActivityScope : IDisposable
    {
        #region constructors

        public TraceActivityScope(TraceServiceBase traceService, int eventId, string message) :
            this(traceService, eventId,
             string.Format("Starting: [{0}]", message),
             string.Format("Ending: [{0}]", message))
        {

        }

        public TraceActivityScope(TraceServiceBase traceService, int eventId, string startMessage, string endMessage)
        {
            TraceService = traceService;

            var activityId = Guid.NewGuid();
            EventId = eventId;

            EndMessage = endMessage;
            OldActivityId = TraceService.CurrentActivityId;

            TraceService.TraceActivityTransfer(eventId, startMessage, activityId);
            TraceService.CurrentActivityId = activityId;
            TraceService.TraceActivityStart(eventId, startMessage);
        }

        #endregion

        #region properties

        public string EndMessage { get; set; }

        public Guid OldActivityId { get; set; }
        public int EventId { get; set; }

        public TraceServiceBase TraceService { get; set; }

        #endregion

        #region methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TraceService.TraceActivityStop(EventId, EndMessage);
                TraceService.TraceActivityTransfer(EventId, EndMessage, OldActivityId);
                TraceService.CurrentActivityId = OldActivityId;
            }
        }

        #endregion
    }
}
