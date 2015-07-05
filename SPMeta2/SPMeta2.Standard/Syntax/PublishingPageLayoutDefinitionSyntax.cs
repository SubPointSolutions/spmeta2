using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class PublishingPageLayoutModelNode : TypedModelNode
    {

    }

    public static class PublishingPageLayoutDefinitionSyntax
    {
        #region methods

        public static TModelNode AddPublishingPageLayout<TModelNode>(this TModelNode model, PublishingPageLayoutDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddPublishingPageLayout(model, definition, null);
        }

        public static TModelNode AddPublishingPageLayout<TModelNode>(this TModelNode model, PublishingPageLayoutDefinition definition,
            Action<PublishingPageLayoutModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddPublishingPageLayouts<TModelNode>(this TModelNode model, IEnumerable<PublishingPageLayoutDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
