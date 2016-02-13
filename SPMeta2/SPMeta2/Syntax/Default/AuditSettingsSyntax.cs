using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class AuditSettingsModelNode : TypedModelNode
    {

    }

    public static class AuditSettingsSyntax
    {
        #region methods

        public static TModelNode AddAuditSettings<TModelNode>(this TModelNode model, AuditSettingsDefinition definition)
            where TModelNode : ModelNode, IAuditSettingsHostModelNode, new()
        {
            return AddAuditSettings(model, definition, null);
        }

        public static TModelNode AddAuditSettings<TModelNode>(this TModelNode model, AuditSettingsDefinition definition,
            Action<AuditSettingsModelNode> action)
            where TModelNode : ModelNode, IAuditSettingsHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
