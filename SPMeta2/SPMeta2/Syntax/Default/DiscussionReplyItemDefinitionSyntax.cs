using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class DiscussionReplyItemModelNode : TypedModelNode, IListItemModelNode,
        ISecurableObjectHostModelNode
    {

    }

    public static class DiscussionReplyItemDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDiscussionReplyItem<TModelNode>(this TModelNode model, DiscussionReplyItemDefinition definition)
            where TModelNode : ModelNode, IDiscussionItemModelNode, new()
        {
            return AddDiscussionReplyItem(model, definition, null);
        }

        public static TModelNode AddDiscussionReplyItem<TModelNode>(this TModelNode model, DiscussionReplyItemDefinition definition,
            Action<DiscussionReplyItemModelNode> action)
            where TModelNode : ModelNode, IDiscussionItemModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddDiscussionReplyItems<TModelNode>(this TModelNode model, IEnumerable<DiscussionReplyItemDefinition> definitions)
           where TModelNode : ModelNode, IDiscussionItemModelNode, new()
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
