using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class InformationRightsManagementSettingsDefinitionSyntax
    {
        #region methods

        public static ModelNode AddInformationRightsManagementSettings(this ModelNode model, InformationRightsManagementSettingsDefinition definition)
        {
            return AddInformationRightsManagementSettings(model, definition, null);
        }

        public static ModelNode AddInformationRightsManagementSettings(this ModelNode model, InformationRightsManagementSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
