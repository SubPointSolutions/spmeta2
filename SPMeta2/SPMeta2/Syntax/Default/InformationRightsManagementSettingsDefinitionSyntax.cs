using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class InformationRightsManagementSettingsModelNode : ListItemModelNode
    {

    }

    public static class InformationRightsManagementSettingsDefinitionSyntax
    {
        public static TModelNode AddInformationRightsManagementSettings<TModelNode>(this TModelNode model, InformationRightsManagementSettingsDefinition definition)
            where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return AddInformationRightsManagementSettings(model, definition, null);
        }

        public static TModelNode AddInformationRightsManagementSettings<TModelNode>(this TModelNode model, InformationRightsManagementSettingsDefinition definition,
            Action<InformationRightsManagementSettingsModelNode> action)
            where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }
    }
}
