using System;

namespace SPMeta2.Containers.Exceptions
{
    [Serializable]
    public class SPMeta2UnsupportedRunnerException : Exception
    {
        public SPMeta2UnsupportedRunnerException() { }
        public SPMeta2UnsupportedRunnerException(string message) : base(message) { }
        public SPMeta2UnsupportedRunnerException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2UnsupportedRunnerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class SPMeta2UnsupportedSSOMRunnerException : SPMeta2UnsupportedRunnerException
    {
        public SPMeta2UnsupportedSSOMRunnerException() { }
        public SPMeta2UnsupportedSSOMRunnerException(string message) : base(message) { }
        public SPMeta2UnsupportedSSOMRunnerException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2UnsupportedSSOMRunnerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class SPMeta2UnsupportedCSOMRunnerException : SPMeta2UnsupportedRunnerException
    {
        public SPMeta2UnsupportedCSOMRunnerException() { }
        public SPMeta2UnsupportedCSOMRunnerException(string message) : base(message) { }
        public SPMeta2UnsupportedCSOMRunnerException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2UnsupportedCSOMRunnerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class SPMeta2UnsupportedO365RunnerException : Exception
    {
        public SPMeta2UnsupportedO365RunnerException() { }
        public SPMeta2UnsupportedO365RunnerException(string message) : base(message) { }
        public SPMeta2UnsupportedO365RunnerException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2UnsupportedO365RunnerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
