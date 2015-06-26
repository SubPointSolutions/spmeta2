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
    public static class RegionalSettingsDefinitionSyntax
    {
        #region methods

        public static WebModelNode AddRegionalSettings(this WebModelNode model, RegionalSettingsDefinition definition)
        {
            return AddRegionalSettings(model, definition, null);
        }

        public static WebModelNode AddRegionalSettings(this WebModelNode model, RegionalSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
