using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Exceptions
{
    [Serializable]
    public class SPMeta2UnsupportedModelHostException : SPMeta2Exception
    {
        public SPMeta2UnsupportedModelHostException() { }
        public SPMeta2UnsupportedModelHostException(string message) : base(message) { }
        public SPMeta2UnsupportedModelHostException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2UnsupportedModelHostException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
