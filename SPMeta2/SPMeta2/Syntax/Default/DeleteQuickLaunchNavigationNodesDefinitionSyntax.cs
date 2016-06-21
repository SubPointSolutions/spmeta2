using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class DeleteQuickLaunchNavigationNodesModelNode : WebModelNode
    {

    }

    public static class DeleteQuickLaunchNavigationNodesModelNodeSyntax
    {
        #region methods

        public static TModelNode AddDeleteQuickLaunchNavigationNodes<TModelNode>(this TModelNode model,
            DeleteQuickLaunchNavigationNodesDefinition definition)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return AddDeleteQuickLaunchNavigationNodes(model, definition, null);
        }

        public static TModelNode AddDeleteQuickLaunchNavigationNodes<TModelNode>(this TModelNode model,
            DeleteQuickLaunchNavigationNodesDefinition definition,
           Action<DeleteQuickLaunchNavigationNodesModelNode> action)
           where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
