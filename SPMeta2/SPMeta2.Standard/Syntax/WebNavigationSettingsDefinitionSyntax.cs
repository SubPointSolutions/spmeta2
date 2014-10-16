using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class WebNavigationSettingsDefinitionSyntax
    {
        #region methods

        public static ModelNode AddWebNavigationSettings(this ModelNode model, WebNavigationSettingsDefinition definition)
        {
            return AddWebNavigationSettings(model, definition, null);
        }

        public static ModelNode AddWebNavigationSettings(this ModelNode model, WebNavigationSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region model

        //public static IEnumerable<WebPartPageDefinition> GetWebPartPages(this DefinitionBase model)
        //{
        //    return model.GetChildModelsAsType<WebPartPageDefinition>();
        //}

        #endregion
    }
}
