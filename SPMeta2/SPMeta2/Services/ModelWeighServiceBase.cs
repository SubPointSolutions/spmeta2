using SPMeta2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Services
{
    public abstract class ModelWeighServiceBase
    {
        public abstract IEnumerable<ModelWeigh> GetModelWeighs();
    }
}
