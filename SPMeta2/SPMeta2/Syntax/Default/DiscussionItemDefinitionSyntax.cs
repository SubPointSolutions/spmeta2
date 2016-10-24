using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class DiscussionItemModelNode : TypedModelNode, IListItemModelNode,
        ISecurableObjectHostModelNode,
        IDiscussionItemModelNode
    {

    }

    public static class DiscussionItemDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDiscussionItem<TModelNode>(this TModelNode model, DiscussionItemDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddDiscussionItem(model, definition, null);
        }

        public static TModelNode AddDiscussionItem<TModelNode>(this TModelNode model, DiscussionItemDefinition definition,
            Action<DiscussionItemModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddDiscussionItems<TModelNode>(this TModelNode model, IEnumerable<DiscussionItemDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region add host

        

        #endregion
    }
}
