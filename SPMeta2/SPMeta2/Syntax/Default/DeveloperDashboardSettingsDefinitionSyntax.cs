using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class DeveloperDashboardSettingsModelNode : TypedModelNode
    {

    }

    public static class DeveloperDashboardSettingsDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDeveloperDashboardSettings<TModelNode>(this TModelNode model, DeveloperDashboardSettingsDefinition definition)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return AddDeveloperDashboardSettings(model, definition, null);
        }

        public static TModelNode AddDeveloperDashboardSettings<TModelNode>(this TModelNode model, DeveloperDashboardSettingsDefinition definition,
            Action<DeveloperDashboardSettingsModelNode> action)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        #endregion
    }
}
