using System;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Exceptions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
    public class SPMeta2ModelValidationException : SPMeta2Exception
    {
        public SPMeta2ModelValidationException() { }
        public SPMeta2ModelValidationException(string message) : base(message) { }
        public SPMeta2ModelValidationException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2ModelValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public DefinitionBase Definition { get; set; }
        public ModelNode Model { get; set; }
    }
}
