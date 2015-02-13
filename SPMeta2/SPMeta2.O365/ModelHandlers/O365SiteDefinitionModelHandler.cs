using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Online.SharePoint.TenantAdministration;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.O365.ModelHosts;
using SPMeta2.Utils;
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using SPMeta2.O365.Definitions;
using SPMeta2.O365.ModelHandlers.Base;
using SPMeta2.Common;
using System.Threading;

namespace SPMeta2.O365.ModelHandlers
{
    public class O365SiteDefinitionModelHandler : O365ModelHandlerBase
    {
        #region properties

        public static int DefaultTimeoutInMilliseconds = 1000 * 60 * 20;
        public static int DefaultPingTimeoutInMilliseconds = 1000 * 10;

        public override Type TargetType
        {
            get { return typeof(O365SiteDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<AdminSiteModelHost>("modelHost", value => value.RequireNotNull());
            var o365SiteModel = model.WithAssertAndCast<O365SiteDefinition>("model", value => value.RequireNotNull());

            DeployO365SiteCollection(modelHost, siteModelHost.HostClientContext, o365SiteModel);
        }

        private void DeployO365SiteCollection(object modelHost, ClientContext context, O365SiteDefinition o365SiteModel)
        {
            var doesSiteExist = false;
            var currentSiteCollection = (Site)null;

            WithExistingSiteAndWeb(context, o365SiteModel.Url, (site, web) =>
            {
                doesSiteExist = site != null;

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = site,
                    ObjectType = typeof(Site),
                    ObjectDefinition = o365SiteModel,
                    ModelHost = modelHost
                });

                if (doesSiteExist)
                {
                    // TODO, some updates if possible


                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = site,
                        ObjectType = typeof(Site),
                        ObjectDefinition = o365SiteModel,
                        ModelHost = modelHost
                    });
                }
            });

            if (doesSiteExist)
                return;

            CreateNewSiteCollection(modelHost, context, o365SiteModel);
        }

        private void CreateNewSiteCollection(object modelHost, ClientContext context, O365SiteDefinition o365SiteModel)
        {
            var tenant = new Tenant(context);
            var siteCollectionProperties = MapSiteCollectionProperties(o365SiteModel);

            tenant.CreateSite(siteCollectionProperties);

            context.Load(tenant);
            context.ExecuteQuery();

            // from here site collection is being provisioned by O365 asynchronously
            // we need to have sorta delay with querying new site collection and continue deployment flow once it has been fully provisioned

            var siteCreated = WaitForSiteProvision(context, o365SiteModel.Url);

            if (siteCreated)
            {
                WithExistingSiteAndWeb(context, o365SiteModel.Url, (site, web) =>
                {
                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = site,
                        ObjectType = typeof(Site),
                        ObjectDefinition = o365SiteModel,
                        ModelHost = modelHost
                    });
                });
            }
            else
            {
                // probably, we should throw an exception as site was not created

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = null,
                    ObjectType = typeof(Site),
                    ObjectDefinition = o365SiteModel,
                    ModelHost = modelHost
                });
            }
        }



        private bool WaitForSiteProvision(ClientContext context, string webAbsoluteUrl)
        {
            var siteCreated = false;
            var timeoutExceeded = false;

            var startDate = DateTime.Now;

            do
            {
                siteCreated = DoesWebExist(context, webAbsoluteUrl);
                Thread.Sleep(DefaultPingTimeoutInMilliseconds);

                var currentDate = DateTime.Now;

                timeoutExceeded = (currentDate - startDate).TotalMilliseconds > DefaultTimeoutInMilliseconds;
            }
            while (!siteCreated && !timeoutExceeded);

            return siteCreated;
        }

        private SiteCreationProperties MapSiteCollectionProperties(O365SiteDefinition o365SiteModel)
        {
            return new SiteCreationProperties()
            {
                Url = o365SiteModel.Url,
                Owner = o365SiteModel.Owner,
                Template = o365SiteModel.Template
            };
        }

        private void WithExistingSiteAndWeb(ClientContext context, string absoluteUrl, Action<Site, Web> action)
        {
            try
            {
                using (var tmp = new ClientContext(absoluteUrl))
                {
                    tmp.Credentials = context.Credentials;

                    tmp.Load(tmp.Site);
                    tmp.Load(tmp.Web);

                    tmp.ExecuteQuery();

                    action(tmp.Site, tmp.Web);
                }
            }
            catch (Exception ex)
            {
                action(null, null);
            }
        }

        private bool DoesWebExist(ClientContext context, string absoluteUrl)
        {
            var result = false;

            WithExistingSiteAndWeb(context, absoluteUrl, (site, web) =>
            {
                result = web != null;
            });

            return result;
        }

        #endregion
    }
}
