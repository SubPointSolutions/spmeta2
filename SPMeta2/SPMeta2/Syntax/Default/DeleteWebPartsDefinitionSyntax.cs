using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class DeleteWebPartsModelNode : ListItemModelNode
    {

    }

    public static class DeleteWebPartsDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDeleteWebParts<TModelNode>(this TModelNode model, DeleteWebPartsDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddDeleteWebParts(model, definition, null);
        }

        public static TModelNode AddDeleteWebParts<TModelNode>(this TModelNode model, DeleteWebPartsDefinition definition,
            Action<DeleteWebPartsModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
