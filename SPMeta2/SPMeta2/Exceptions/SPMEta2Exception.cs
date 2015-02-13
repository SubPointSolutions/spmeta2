using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Exceptions
{
    [Serializable]
    public class SPMeta2Exception : ApplicationException
    {
        #region constructors

        public SPMeta2Exception() { }
        public SPMeta2Exception(string message) : base(message) { }
        public SPMeta2Exception(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2Exception(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
