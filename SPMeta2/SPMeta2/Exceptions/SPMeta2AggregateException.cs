using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SPMeta2.Exceptions
{

#if  NET35

    [Serializable]
    public class SPMeta2AggregateException : SPMeta2Exception
    {
        public SPMeta2AggregateException() { }

        public SPMeta2AggregateException(IEnumerable<Exception> exceptions)
            : this(String.Empty, exceptions)
        {
            
        }

        public SPMeta2AggregateException(string message, IEnumerable<Exception> exceptions)
            : base(message)
        {
            InnerExceptions = new ReadOnlyCollection<Exception>(exceptions.ToList());
        }

        public SPMeta2AggregateException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2AggregateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public ReadOnlyCollection<Exception> InnerExceptions { get; private set; }
    }

#endif

#if !NET35

    [Serializable]
    public class SPMeta2AggregateException : AggregateException
    {
        public SPMeta2AggregateException(IEnumerable<Exception> exceptions)
            : base(exceptions)
        {

        }

        public SPMeta2AggregateException(string message, IEnumerable<Exception> exceptions)
            : base(message, exceptions)
        {

        }
    }

#endif

}
