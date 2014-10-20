using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class WikiPageDefinitionSyntax
    {
        #region methods

        public static ModelNode AddWikiPage(this ModelNode model, WikiPageDefinition definition)
        {
            return AddWikiPage(model, definition, null);
        }

        public static ModelNode AddWikiPage(this ModelNode model, WikiPageDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region host override

        public static ModelNode AddHostWikiPage(this ModelNode model, WikiPageDefinition definition)
        {
            return AddHostWikiPage(model, definition, null);
        }

        public static ModelNode AddHostWikiPage(this ModelNode model, WikiPageDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
