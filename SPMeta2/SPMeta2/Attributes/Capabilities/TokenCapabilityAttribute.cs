using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Attributes.Capabilities
{
    public class TokenCapabilityAttribute : CapabilityAttribute
    {
        public string Token { get; set; }
        public string Comment { get; set; }
    }
}
