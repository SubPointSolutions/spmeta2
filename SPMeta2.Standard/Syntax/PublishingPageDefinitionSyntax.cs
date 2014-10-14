using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class PublishingPageDefinitionSyntax
    {
        public static ModelNode AddPublishingPage(this ModelNode model, PublishingPageDefinition definition)
        {
            return AddPublishingPage(model, definition, null);
        }

        public static ModelNode AddPublishingPage(this ModelNode model, PublishingPageDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }
    }
}
