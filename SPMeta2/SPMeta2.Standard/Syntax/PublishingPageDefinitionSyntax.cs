using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class PublishingPageDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddPublishingPage(this ModelNode model, PublishingPageDefinition definition)
        {
            return AddPublishingPage(model, definition, null);
        }

        public static ModelNode AddPublishingPage(this ModelNode model, PublishingPageDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region host override

        public static ModelNode AddHostPublishingPage(this ModelNode model, PublishingPageDefinition definition)
        {
            return AddHostPublishingPage(model, definition, null);
        }

        public static ModelNode AddHostPublishingPage(this ModelNode model, PublishingPageDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
