using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Exceptions;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class PrefixModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PrefixDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var prefixDefinition = model.WithAssertAndCast<PrefixDefinition>("model", value => value.RequireNotNull());

            DeployPrefix(webAppModelHost.HostWebApplication, prefixDefinition);
        }

        protected SPPrefix GetPrefix(SPWebApplication webApp, PrefixDefinition prefixDefinition)
        {
            var prefixes = webApp.Prefixes;
            return prefixes.FirstOrDefault(p => p.Name.ToUpper() == prefixDefinition.Path.ToUpper());
        }

        private void DeployPrefix(SPWebApplication webApp, PrefixDefinition prefixDefinition)
        {
            var prefixes = webApp.Prefixes;
            var existingPrefix = GetPrefix(webApp, prefixDefinition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingPrefix,
                ObjectType = typeof(SPPrefix),
                ObjectDefinition = prefixDefinition,
                ModelHost = webApp
            });

            if (existingPrefix == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new prefix");

                var prefixType = (SPPrefixType)Enum.Parse(typeof(SPPrefixType), prefixDefinition.PrefixType, true);
                existingPrefix = webApp.Prefixes.Add(prefixDefinition.Path, prefixType);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing prefix");
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingPrefix,
                ObjectType = typeof(SPPrefix),
                ObjectDefinition = prefixDefinition,
                ModelHost = webApp
            });

        }

        #endregion
    }
}
