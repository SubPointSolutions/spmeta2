using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{

    public class PageLayoutAndSiteTemplateSettingsModelNode : TypedModelNode
    {

    }

    public static class PageLayoutAndSiteTemplateSettingsDefinitionSyntax
    {
        #region methods

        public static WebModelNode AddPageLayoutAndSiteTemplateSettings(this WebModelNode model, PageLayoutAndSiteTemplateSettingsDefinition definition)
        {
            return AddPageLayoutAndSiteTemplateSettings(model, definition, null);
        }

        public static WebModelNode AddPageLayoutAndSiteTemplateSettings(this WebModelNode model, PageLayoutAndSiteTemplateSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
