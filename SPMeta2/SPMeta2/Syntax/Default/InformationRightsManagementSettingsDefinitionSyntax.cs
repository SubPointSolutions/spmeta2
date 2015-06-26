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

        public static ListModelNode AddInformationRightsManagementSettings(this ListModelNode model, InformationRightsManagementSettingsDefinition definition)
        {
            return AddInformationRightsManagementSettings(model, definition, null);
        }

        public static ListModelNode AddInformationRightsManagementSettings(this ListModelNode model, InformationRightsManagementSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
