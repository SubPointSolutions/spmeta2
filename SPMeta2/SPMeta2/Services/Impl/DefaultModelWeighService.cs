using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Common;

namespace SPMeta2.Services.Impl
{
    public class DefaultModelWeighService : ModelWeighServiceBase
    {
        public override IEnumerable<Common.ModelWeigh> GetModelWeighs()
        {
            return DefaultModelWeigh.Weighs;
        }
    }
}
