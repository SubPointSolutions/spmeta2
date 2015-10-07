using System.Collections.Generic;
using SPMeta2.Common;
using SPMeta2.Models;

namespace SPMeta2.Services
{
    public abstract class ModelWeighServiceBase
    {
        #region methods

        public abstract IEnumerable<ModelWeigh> GetModelWeighs();

        public virtual void SortChildModelNodes(ModelNode modelNode, List<ModelNode> childNodes)
        {

        }

        #endregion
    }
}
