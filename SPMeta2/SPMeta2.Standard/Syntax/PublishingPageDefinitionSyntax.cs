using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class PublishingPageModelNode : TypedModelNode,
        IWebpartHostModelNode,
        ISecurableObjectHostModelNode
    {

    }

    public static class PublishingPageDefinitionSyntax
    {
        #region methods

        public static TModelNode AddPublishingPage<TModelNode>(this TModelNode model, PublishingPageDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddPublishingPage(model, definition, null);
        }

        public static TModelNode AddPublishingPage<TModelNode>(this TModelNode model, PublishingPageDefinition definition,
            Action<PublishingPageModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddPublishingPages<TModelNode>(this TModelNode model, IEnumerable<PublishingPageDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region host override

        public static TModelNode AddHostPublishingPage<TModelNode>(this TModelNode model, PublishingPageDefinition definition)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddHostPublishingPage(model, definition, null);
        }
        public static TModelNode AddHostPublishingPage<TModelNode>(this TModelNode model, PublishingPageDefinition definition,
            Action<PublishingPageModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
