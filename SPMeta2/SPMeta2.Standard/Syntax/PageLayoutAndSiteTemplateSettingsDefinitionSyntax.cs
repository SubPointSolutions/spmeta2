using System;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class PageLayoutAndSiteTemplateSettingsModelNode : TypedModelNode
    {

    }

    public static class PageLayoutAndSiteTemplateSettingsDefinitionSyntax
    {
        #region methods

        public static TModelNode AddPageLayoutAndSiteTemplateSettings<TModelNode>(this TModelNode model, PageLayoutAndSiteTemplateSettingsDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddPageLayoutAndSiteTemplateSettings(model, definition, null);
        }

        public static TModelNode AddPageLayoutAndSiteTemplateSettings<TModelNode>(this TModelNode model, PageLayoutAndSiteTemplateSettingsDefinition definition,
            Action<PageLayoutAndSiteTemplateSettingsModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
