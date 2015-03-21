using SPMeta2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
