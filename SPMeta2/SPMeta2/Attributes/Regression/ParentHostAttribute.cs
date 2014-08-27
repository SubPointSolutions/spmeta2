using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Attributes.Regression
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParentHostAttribute : Attribute
    {
        public ParentHostAttribute(Type hostType)
        {
            HostType = hostType;
        }

        public Type HostType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RootHostAttribute : Attribute
    {
        public RootHostAttribute(Type hostType)
        {
            HostType = hostType;
        }

        public Type HostType { get; set; }
    }
}
