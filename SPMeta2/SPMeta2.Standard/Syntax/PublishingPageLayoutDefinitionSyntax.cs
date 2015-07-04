using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class PublishingPageLayoutModelNode : TypedModelNode
    {

    }

    public static class PublishingPageLayoutDefinitionSyntax
    {
        #region publishing page

        public static ListModelNode AddPublishingPageLayout(this ListModelNode model,
            PublishingPageLayoutDefinition definition)
        {
            return AddPublishingPageLayout(model, definition, null);
        }

        public static ListModelNode AddPublishingPageLayout(this ListModelNode model,
            PublishingPageLayoutDefinition definition, Action<PublishingPageLayoutModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddPublishingPageLayouts(this ModelNode model, IEnumerable<PublishingPageLayoutDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
