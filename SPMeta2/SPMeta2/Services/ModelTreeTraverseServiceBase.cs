using System;
using System.Collections.Generic;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;

namespace SPMeta2.Services
{
    public class ModelTreeTraverseServiceExceptionEventArgs : EventArgs
    {
        public ModelTreeTraverseServiceExceptionEventArgs()
        {

        }

        public bool Handled { get; set; }

        public Exception Exception { get; set; }
    }

    public abstract class ModelTreeTraverseServiceBase : SPMetaServiceBase
    {
        #region methods

        public EventHandler<ModelTreeTraverseServiceExceptionEventArgs> OnException;

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
