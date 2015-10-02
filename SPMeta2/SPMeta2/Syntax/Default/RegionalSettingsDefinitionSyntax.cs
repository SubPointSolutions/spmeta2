using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class RegionalSettingsModelNode : TypedModelNode
    {

    }

    public static class RegionalSettingsDefinitionSyntax
    {
        #region methods

        public static TModelNode AddRegionalSettings<TModelNode>(this TModelNode model, RegionalSettingsDefinition definition)
            where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return AddRegionalSettings(model, definition, null);
        }

        public static TModelNode AddRegionalSettings<TModelNode>(this TModelNode model, RegionalSettingsDefinition definition,
            Action<RegionalSettingsModelNode> action)
            where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
