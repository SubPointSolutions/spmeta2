using System;
using SPMeta2.Models;

namespace SPMeta2.Exceptions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable] 
    public class SPMeta2ModelDeploymentException : SPMeta2Exception
    {
        public SPMeta2ModelDeploymentException() { }
        public SPMeta2ModelDeploymentException(string message) : base(message) { }
        public SPMeta2ModelDeploymentException(string message, Exception inner) : base(message, inner) { }
        protected SPMeta2ModelDeploymentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


        public ModelNode ModelNode { get; set; }
    }
}
