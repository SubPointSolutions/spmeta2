using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class PageLayoutAndSiteTemplateSettingsDefinitionSyntax
    {
        #region methods

        public static ModelNode AddPageLayoutAndSiteTemplateSettings(this ModelNode model, PageLayoutAndSiteTemplateSettingsDefinition definition)
        {
            return AddPageLayoutAndSiteTemplateSettings(model, definition, null);
        }

        public static ModelNode AddPageLayoutAndSiteTemplateSettings(this ModelNode model, PageLayoutAndSiteTemplateSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
