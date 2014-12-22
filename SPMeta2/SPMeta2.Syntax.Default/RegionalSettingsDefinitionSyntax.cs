using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class RegionalSettingsDefinitionSyntax
    {
        #region methods

        public static ModelNode AddRegionalSettings(this ModelNode model, RegionalSettingsDefinition definition)
        {
            return AddRegionalSettings(model, definition, null);
        }

        public static ModelNode AddRegionalSettings(this ModelNode model, RegionalSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
