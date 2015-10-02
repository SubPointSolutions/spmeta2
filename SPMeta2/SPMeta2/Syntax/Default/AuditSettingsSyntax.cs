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
