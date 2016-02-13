using System;

namespace SPMeta2.Attributes.Capabilities
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SelfHostCapabilityAttribute : CapabilityAttribute
    {

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParentHostCapabilityAttribute : CapabilityAttribute
    {
        #region constructors
        public ParentHostCapabilityAttribute(Type hostType)
        {
            HostType = hostType;
        }

        #endregion

        #region properties

        public bool IsRoot { get; set; }

        public Type HostType { get; set; }

        #endregion
    }
}
