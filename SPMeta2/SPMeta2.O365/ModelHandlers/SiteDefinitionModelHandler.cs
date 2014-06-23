using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Online.SharePoint.TenantAdministration;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.O365.ModelHosts;
using SPMeta2.Utils;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;

namespace SPMeta2.O365.ModelHandlers
{
    public class SiteDefinitionModelHandler : ModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SiteDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            //var siteModel = model.WithAssertAndCast<SiteDefinition>("model", value => value.RequireNotNull());

            var context = siteModelHost.HostClientContext;

            var tenant = new Tenant(context);
            var properties = new SiteCreationProperties()
            {
                Url = "https://<TENANT>.sharepoint.com/sites/site1",
                Owner = "<USER>@<TENANT>.onmicrosoft.com",
                Template = "STS#0",
                StorageMaximumLevel = 1000,
                UserCodeMaximumLevel = 300
            };
            tenant.CreateSite(properties);

            context.Load(tenant);
            context.ExecuteQuery();
        }

        #endregion
    }
}
