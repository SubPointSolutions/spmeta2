using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.Services
{
    public abstract class DefinitionIdentityServiceBase
    {
        public abstract string GetIdentityKey(DefinitionBase definition);
    }
}
