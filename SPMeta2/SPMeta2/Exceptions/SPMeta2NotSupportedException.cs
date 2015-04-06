using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Exceptions
{
    [Serializable] 
    public class SPMeta2NotSupportedException : SPMeta2Exception
    {
        public SPMeta2NotSupportedException() { }
        public SPMeta2NotSupportedException(string message) : base(message) { }
        public SPMeta2NotSupportedException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2NotSupportedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
