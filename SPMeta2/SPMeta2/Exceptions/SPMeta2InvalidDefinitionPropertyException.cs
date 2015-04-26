using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Exceptions
{
    [Serializable]
    public class SPMeta2InvalidDefinitionPropertyException : SPMeta2Exception
    {
        public SPMeta2InvalidDefinitionPropertyException() { }
        public SPMeta2InvalidDefinitionPropertyException(string message) : base(message) { }
        public SPMeta2InvalidDefinitionPropertyException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2InvalidDefinitionPropertyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
