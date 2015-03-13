using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class PublishingPageLayoutDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddPublishingPageLayout(this ModelNode model, PublishingPageLayoutDefinition definition)
        {
            return AddPublishingPageLayout(model, definition, null);
        }

        public static ModelNode AddPublishingPageLayout(this ModelNode model, PublishingPageLayoutDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
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
