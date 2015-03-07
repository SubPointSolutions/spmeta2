using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Tests.Services
{
    public class FakeInMemoryTraceService : TraceServiceBase
    {
        #region constructors

        public FakeInMemoryTraceService()
        {
            CriticalMessages = new List<LogMessage>();
            ErrorMessages = new List<LogMessage>();
            WarningMessages = new List<LogMessage>();
            InformationMessages = new List<LogMessage>();
            VerboseMessages = new List<LogMessage>();
        }

        #endregion

        #region classes

        public class LogMessage
        {
            public int Id { get; set; }
            public object Message { get; set; }
            public Exception Exception { get; set; }
        }

        #endregion

        #region properties

        public List<LogMessage> CriticalMessages { get; set; }
        public List<LogMessage> ErrorMessages { get; set; }
        public List<LogMessage> WarningMessages { get; set; }
        public List<LogMessage> InformationMessages { get; set; }
        public List<LogMessage> VerboseMessages { get; set; }

        #endregion


        public override void Critical(int id, object message, Exception exception)
        {
            CriticalMessages.Add(new LogMessage
            {
                Id = id,
                Message = message,
                Exception = exception
            });
        }

        public override void Error(int id, object message, Exception exception)
        {
            ErrorMessages.Add(new LogMessage
            {
                Id = id,
                Message = message,
                Exception = exception
            });
        }

        public override void Warning(int id, object message, Exception exception)
        {
            WarningMessages.Add(new LogMessage
            {
                Id = id,
                Message = message,
                Exception = exception
            });
        }

        public override void Information(int id, object message, Exception exception)
        {
            InformationMessages.Add(new LogMessage
            {
                Id = id,
                Message = message,
                Exception = exception
            });
        }

        public override void Verbose(int id, object message, Exception exception)
        {
            VerboseMessages.Add(new LogMessage
            {
                Id = id,
                Message = message,
                Exception = exception
            });
        }

        public override void TraceActivityStart(int id, object message)
        {

        }

        public override void TraceActivityStop(int id, object message)
        {

        }

        public override void TraceActivityTransfer(int id, object message, Guid relatedActivityId)
        {

        }

        public override Guid CurrentActivityId
        {
            get { return Guid.Empty; }
            set
            {

            }
        }
    }
}
