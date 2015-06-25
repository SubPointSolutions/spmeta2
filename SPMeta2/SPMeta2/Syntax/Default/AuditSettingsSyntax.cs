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
    public static class AuditSettingsSyntax
    {
        #region methods

        public static ListModelNode AddAuditSettings(this ListModelNode model, AuditSettingsDefinition definition)
        {
            return AddAuditSettings(model, definition, null);
        }

        public static ListModelNode AddAuditSettings(this ListModelNode model, AuditSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static SiteModelNode AddAuditSettings(this SiteModelNode model, AuditSettingsDefinition definition)
        {
            return AddAuditSettings(model, definition, null);
        }

        public static SiteModelNode AddAuditSettings(this SiteModelNode model, AuditSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddAuditSettings(this WebModelNode model, AuditSettingsDefinition definition)
        {
            return AddAuditSettings(model, definition, null);
        }

        public static WebModelNode AddAuditSettings(this WebModelNode model, AuditSettingsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
