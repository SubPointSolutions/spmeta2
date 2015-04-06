using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Exceptions
{
    [Serializable] 
    public class SPMeta2NotImplementedException : SPMeta2Exception
    {
        public SPMeta2NotImplementedException() { }
        public SPMeta2NotImplementedException(string message) : base(message) { }
        public SPMeta2NotImplementedException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2NotImplementedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
