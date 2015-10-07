using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class WebPartPageModelNode : TypedModelNode,
        IWebpartHostModelNode,
        ISecurableObjectHostModelNode
    {

    }

    public static class WebPartPageDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWebPartPage<TModelNode>(this TModelNode model, WebPartPageDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddWebPartPage(model, definition, null);
        }

        public static TModelNode AddWebPartPage<TModelNode>(this TModelNode model, WebPartPageDefinition definition,
            Action<WebPartPageModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddWebPartPages<TModelNode>(this TModelNode model, IEnumerable<WebPartPageDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region host override

        public static TModelNode AddHostWebPartPage<TModelNode>(this TModelNode model, WebPartPageDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddHostWebPartPage(model, definition, null);
        }
        public static TModelNode AddHostWebPartPage<TModelNode>(this TModelNode model, WebPartPageDefinition definition,
            Action<WebPartPageModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
