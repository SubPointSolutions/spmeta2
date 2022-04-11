using System;
using System.Runtime.Serialization;

using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class PeoplePickerSettingsModelNode : TypedModelNode
    {
        
    }

    public static class PeoplePickerSettingsDefintionSyntax
    {
        #region methods

        public static TModelNode AddPeoplePickerSettings<TModelNode>(this TModelNode model, PeoplePickerSettingsDefinition definition)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return AddPeoplePickerSettings(model, definition, null);
        }

        public static TModelNode AddPeoplePickerSettings<TModelNode>(this TModelNode model, PeoplePickerSettingsDefinition definition,
            Action<PeoplePickerSettingsModelNode> action)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}