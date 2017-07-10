using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace SPMeta2.Regression.Service
{
    public class RegressionTraceService : TraceServiceBase
    {
        #region consturctors

        public RegressionTraceService()
        {
            IsVerboseEnabled = false;

            IsInformationEnabled = true;
            IsWarningEnabled = true;
            IsErrorEnabled = true;
            IsCriticalEnabled = true;

            var domainId = AppDomain.CurrentDomain.Id;
            var threadId = Thread.CurrentThread.ManagedThreadId;

            var fileName = string.Format("spmeta2.regression.{0}-{1}.{2}.log", domainId, threadId, GetTimestamp());
            LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        #endregion

        #region classes

        protected enum Level
        {
            Critical,
            Error,
            Warning,
            Information,
            Verbose
        }

        #endregion

        #region props

        public string LogFilePath { get; set; }

        #endregion

        #region methods

        protected virtual String GetTimestamp()
        {
            return (DateTime.Now).ToString("yyyyMMdd_HHmmssfff");
        }

        protected virtual void InternalWrite(Level level, string message)
        {
            var internalMessgae = string.Format("[{0}]: {1}", level, message);

            Trace.WriteLine(internalMessgae);
            Debug.WriteLine(internalMessgae);

            using (StreamWriter sw = File.AppendText(LogFilePath))
                sw.WriteLine(internalMessgae);
        }

        public override void Critical(int id, object message, Exception exception)
        {
            if (IsCriticalEnabled)
                InternalWrite(Level.Critical, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Error(int id, object message, Exception exception)
        {
            if (IsErrorEnabled)
                InternalWrite(Level.Error, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Warning(int id, object message, Exception exception)
        {
            if (IsWarningEnabled)
                InternalWrite(Level.Warning, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Information(int id, object message, Exception exception)
        {
            if (IsInformationEnabled)
                InternalWrite(Level.Information, message + ((exception != null) ? exception.ToString() : string.Empty));
        }

        public override void Verbose(int id, object message, Exception exception)
        {
            if (IsVerboseEnabled)
                InternalWrite(Level.Verbose, message + ((exception != null) ? exception.ToString() : string.Empty));
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


        private Guid? _currentActivityId;

        public override Guid CurrentActivityId
        {
            get
            {
                if (_currentActivityId == null)
                    _currentActivityId = Guid.NewGuid();

                return _currentActivityId.Value;
            }
            set
            {
                _currentActivityId = value;
            }
        }
        #endregion
    }
}
