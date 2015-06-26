using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Attributes.Capabilities
{
    public class WebTokenCapabilityAttribute : TokenCapabilityAttribute
    {
        public WebTokenCapabilityAttribute()
        {
            Token = "~site";
        }
    }
}
