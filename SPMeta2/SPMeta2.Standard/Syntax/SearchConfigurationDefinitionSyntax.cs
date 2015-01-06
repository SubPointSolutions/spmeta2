using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class SearchConfigurationDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddSearchConfiguration(this ModelNode model, SearchConfigurationDefinition definition)
        {
            return AddSearchConfiguration(model, definition, null);
        }

        public static ModelNode AddSearchConfiguration(this ModelNode model, SearchConfigurationDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
