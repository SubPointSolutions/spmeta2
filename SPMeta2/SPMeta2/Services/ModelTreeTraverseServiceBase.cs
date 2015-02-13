using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Extensions;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;

namespace SPMeta2.Services
{
    public abstract class ModelTreeTraverseServiceBase : SPMetaServiceBase
    {
        #region methods

        public abstract void Traverse(object modelHost, ModelNode modelNode);

        public Func<ModelNode, ModelHandlerBase> OnModelHandlerResolve { get; set; }

        public Action<ModelNode> OnModelProcessing { get; set; }
        public Action<ModelNode> OnModelProcessed { get; set; }

        public Action<ModelNode> OnModelFullyProcessing { get; set; }
        public Action<ModelNode> OnModelFullyProcessed { get; set; }

        public Action<ModelNode, Type, IEnumerable<ModelNode>> OnChildModelsProcessing { get; set; }
        public Action<ModelNode, Type, IEnumerable<ModelNode>> OnChildModelsProcessed { get; set; }

        #endregion
    }
}
