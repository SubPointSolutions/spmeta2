using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class SearchSettingsDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddSearchSettings(this ModelNode model, SearchSettingsDefinition definition)
        {
            return AddSearchSettings(model, definition, null);
        }

        public static ModelNode AddSearchSettings(this ModelNode model, SearchSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
