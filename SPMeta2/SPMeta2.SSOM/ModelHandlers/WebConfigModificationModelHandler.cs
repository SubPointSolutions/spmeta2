using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebConfigModificationModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebConfigModificationDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebConfigModificationDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(webAppModelHost, webAppModelHost.HostWebApplication, definition);
        }

        private void DeployDefinition(WebApplicationModelHost modelHost, SPWebApplication webApp, WebConfigModificationDefinition definition)
        {
            var existingWebConfig = GetCurrentSPWebConfigModification(webApp, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingWebConfig,
                ObjectType = typeof(SPWebConfigModification),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (existingWebConfig != null)
                webApp.WebConfigModifications.Remove(existingWebConfig);

            existingWebConfig = new SPWebConfigModification();

            MapConfig(existingWebConfig, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingWebConfig,
                ObjectType = typeof(SPWebConfigModification),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            webApp.WebConfigModifications.Add(existingWebConfig);
            webApp.Update();

            //webApp.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();
            webApp.WebService.ApplyWebConfigModifications();
        }

        private void MapConfig(SPWebConfigModification config, WebConfigModificationDefinition definition)
        {
            config.Path = definition.Path;
            config.Name = definition.Name;
            config.Sequence = definition.Sequence;
            config.Owner = definition.Owner;
            config.Type = (SPWebConfigModification.SPWebConfigModificationType)Enum.Parse(typeof(SPWebConfigModification.SPWebConfigModificationType), definition.Type);
            config.Value = definition.Value;
        }

        protected SPWebConfigModification GetCurrentSPWebConfigModification(SPWebApplication webApp, WebConfigModificationDefinition definition)
        {
            return webApp.WebConfigModifications.FirstOrDefault(c => c.Owner == definition.Owner);
        }

        #endregion
    }
}
