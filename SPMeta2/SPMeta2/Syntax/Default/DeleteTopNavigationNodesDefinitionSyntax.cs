using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class DeleteTopNavigationNodesModelNode : WebModelNode
    {

    }

    public static class DeleteTopNavigationNodesDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDeleteTopNavigationNodes<TModelNode>(this TModelNode model,
            DeleteTopNavigationNodesDefinition definition)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return AddDeleteTopNavigationNodes(model, definition, null);
        }

        public static TModelNode AddDeleteTopNavigationNodes<TModelNode>(this TModelNode model,
            DeleteTopNavigationNodesDefinition definition,
           Action<DeleteTopNavigationNodesModelNode> action)
           where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
