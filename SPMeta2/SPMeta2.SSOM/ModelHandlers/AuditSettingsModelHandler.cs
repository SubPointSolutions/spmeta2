using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class AuditSettingsModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(AuditSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<AuditSettingsDefinition>("model", value => value.RequireNotNull());

            DeployAuditSettings(modelHost, definition);
        }

        private void DeployAuditSettings(object modelHost, AuditSettingsDefinition definition)
        {
            var auditObj = GetCurrentAuditObject(modelHost);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = auditObj,
                ObjectType = typeof(SPAudit),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (definition.AuditFlags.Any())
            {
                var mask = SPAuditMaskType.None;

                foreach (var auditFlag in definition.AuditFlags)
                {
                    var flag = (SPAuditMaskType)Enum.Parse(typeof(SPAuditMaskType), auditFlag);
                    mask = mask | flag;
                }

                auditObj.AuditFlags = mask;
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = auditObj,
                ObjectType = typeof(SPAudit),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            auditObj.Update();
        }

        protected SPAudit GetCurrentAuditObject(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.Audit;
            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb.Audit;
            else if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.Audit;
            else
            {
                throw new SPMeta2NotSupportedException(string.Format("model host {0} is not supported", modelHost.GetType()));
            }
        }

        #endregion
    }
}
