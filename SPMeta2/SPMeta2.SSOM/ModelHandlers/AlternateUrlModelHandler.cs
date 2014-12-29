using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class AlternateUrlModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(AlternateUrlDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<AlternateUrlDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, webAppModelHost.HostWebApplication, definition);
        }

        protected SPAlternateUrl GetCurrentAlternateUrl(SPWebApplication webApp, AlternateUrlDefinition definition)
        {
            var alternateUrls = webApp.AlternateUrls;

            var url = definition.Url;
            var urlZone = (SPUrlZone)Enum.Parse(typeof(SPUrlZone), definition.UrlZone);

            return alternateUrls.GetResponseUrl(urlZone);
        }

        private void DeployDefinition(object modelHost, SPWebApplication webApp, AlternateUrlDefinition definition)
        {
            var alternateUrls = webApp.AlternateUrls;

            var url = definition.Url;
            var urlZone = (SPUrlZone)Enum.Parse(typeof(SPUrlZone), definition.UrlZone);

            var responseUrl = GetCurrentAlternateUrl(webApp, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = responseUrl,
                ObjectType = typeof(SPAlternateUrl),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (!string.IsNullOrEmpty(url))
            {
                responseUrl = new SPAlternateUrl(url, urlZone);
                alternateUrls.SetResponseUrl(responseUrl);
            }
            else
            {
                alternateUrls.UnsetResponseUrl(urlZone);
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = responseUrl,
                ObjectType = typeof(SPAlternateUrl),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            alternateUrls.Update();
        }

        #endregion
    }
}
