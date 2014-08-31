using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Attributes.Regression
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DefaultParentHostAttribute : Attribute
    {
        public DefaultParentHostAttribute(Type hostType)
        {
            HostType = hostType;
        }

        public Type HostType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DefaultRootHostAttribute : Attribute
    {
        public DefaultRootHostAttribute(Type hostType)
        {
            HostType = hostType;
        }

        public Type HostType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CSOMParentHostAttribute : DefaultParentHostAttribute
    {
        public CSOMParentHostAttribute(Type hostType)
            : base(hostType)
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CSOMRootHostAttribute : DefaultRootHostAttribute
    {
        public CSOMRootHostAttribute(Type hostType)
            : base(hostType)
        {

        }
    }
}
