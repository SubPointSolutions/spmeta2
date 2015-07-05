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
