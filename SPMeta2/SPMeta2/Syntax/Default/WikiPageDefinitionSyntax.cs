using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class WikiPageModelNode : TypedModelNode,
        IWebpartHostModelNode,
        ISecurableObjectHostModelNode
    {

    }

    public static class WikiPageDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWikiPage<TModelNode>(this TModelNode model, WikiPageDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddWikiPage(model, definition, null);
        }

        public static TModelNode AddWikiPage<TModelNode>(this TModelNode model, WikiPageDefinition definition,
            Action<WikiPageModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddWikiPages<TModelNode>(this TModelNode model, IEnumerable<WikiPageDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region host override

        public static TModelNode AddHostWikiPage<TModelNode>(this TModelNode model, WikiPageDefinition definition)
             where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddHostWikiPage(model, definition, null);
        }
        public static TModelNode AddHostWikiPage<TModelNode>(this TModelNode model, WikiPageDefinition definition,
            Action<WikiPageModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
