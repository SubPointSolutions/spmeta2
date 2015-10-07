using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class ListItemModelNode : TypedModelNode, IListItemModelNode,
        ISecurableObjectHostModelNode
    {

    }

    public static class ListItemDefinitionSyntax
    {
        #region methods

        public static TModelNode AddListItem<TModelNode>(this TModelNode model, ListItemDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddListItem(model, definition, null);
        }

        public static TModelNode AddListItem<TModelNode>(this TModelNode model, ListItemDefinition definition,
            Action<ListItemModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddListItems<TModelNode>(this TModelNode model, IEnumerable<ListItemDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion


        #region add host

        public static TModelNode AddHostListItem<TModelNode>(this TModelNode model, ListItemDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddHostListItem(model, definition, null);
        }
        public static TModelNode AddHostListItem<TModelNode>(this TModelNode model, ListItemDefinition definition,
            Action<ListItemModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
