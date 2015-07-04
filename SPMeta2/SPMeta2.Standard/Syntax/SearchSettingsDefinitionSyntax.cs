using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class SearchSettingsModelNode : TypedModelNode
    {

    }

    public static class SearchSettingsDefinitionSyntax
    {
        #region publishing page

        public static SiteModelNode AddSearchSettings(this SiteModelNode model, SearchSettingsDefinition definition)
        {
            return AddSearchSettings(model, definition, null);
        }

        public static SiteModelNode AddSearchSettings(this SiteModelNode model, SearchSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddSearchSettings(this WebModelNode model, SearchSettingsDefinition definition)
        {
            return AddSearchSettings(model, definition, null);
        }

        public static WebModelNode AddSearchSettings(this WebModelNode model, SearchSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
