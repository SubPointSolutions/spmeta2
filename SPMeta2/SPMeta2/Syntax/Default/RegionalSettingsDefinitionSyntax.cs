using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

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
