using System;
using System.Linq;
using System.Reflection;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class SiteModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(SiteDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webAppModelHost = modelHost.WithAssertAndCast<WebApplicationModelHost>("modelHost", value => value.RequireNotNull());
            var siteModel = model.WithAssertAndCast<SiteDefinition>("model", value => value.RequireNotNull());

            DeploySite(webAppModelHost, webAppModelHost.HostWebApplication, siteModel);
        }

        protected SPSite GetExistingSite(SPWebApplication webApp, SiteDefinition definition)
        {
            var siteCollectionUrl = SPUrlUtility.CombineUrl(definition.PrefixName, definition.Url);

            if (webApp.Sites.Names.Contains(siteCollectionUrl))
                return webApp.Sites[siteCollectionUrl];

            return null;
        }

        public override void WithResolvingModelHost(ModelHostResolveContext context)
        {
            base.WithResolvingModelHost(context);

            ForceInvalidateSite((context.ModelHost as SiteModelHost).HostSite);
        }

        private void ForceInvalidateSite(SPSite site)
        {
            // here we invalidate Receivers Collection as somehow it does not get updated during adding new receivers
            var m_eventReceiversField = typeof(SPSite).GetField("m_eventReceivers", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            m_eventReceiversField.SetValue(site, null);
        }

        private void DeploySite(WebApplicationModelHost webAppModelHost, Microsoft.SharePoint.Administration.SPWebApplication webApp, SiteDefinition siteModel)
        {
            var existingSite = GetExistingSite(webApp, siteModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingSite,
                ObjectType = typeof(SPSite),
                ObjectDefinition = siteModel,
                ModelHost = webAppModelHost
            });

            if (existingSite == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new site");

                var siteCollectionUrl = SPUrlUtility.CombineUrl(siteModel.PrefixName, siteModel.Url);

                if (string.IsNullOrEmpty(siteModel.DatabaseName))
                {
                    var newSite = webApp.Sites.Add(siteCollectionUrl,
                        siteModel.Name,
                        siteModel.Description,
                        siteModel.LCID,
                        siteModel.SiteTemplate,

                        siteModel.OwnerLogin,
                        siteModel.OwnerName,
                        siteModel.OwnerEmail,

                        siteModel.SecondaryContactLogin,
                        siteModel.SecondaryContactName,
                        siteModel.SecondaryContactEmail);

                    // weird issue with second admin, needs to be setup manually
                    newSite.SecondaryContact = newSite.RootWeb.EnsureUser(siteModel.SecondaryContactLogin);
                }
                else
                {
                    // TODO, should be reimplemented later.
                    var dbServerName = webApp.ContentDatabases[0].Server;

                    var newSite = webApp.Sites.Add(siteCollectionUrl,
                        siteModel.Name,
                        siteModel.Description,
                        siteModel.LCID,
                        siteModel.SiteTemplate,

                        siteModel.OwnerLogin,
                        siteModel.OwnerName,
                        siteModel.OwnerEmail,

                        siteModel.SecondaryContactLogin,
                        siteModel.SecondaryContactName,
                        siteModel.SecondaryContactEmail,

                        dbServerName,
                        siteModel.DatabaseName,
                        null,
                        null);

                    // weird issue with second admin, needs to be setup manually
                    newSite.SecondaryContact = newSite.RootWeb.EnsureUser(siteModel.SecondaryContactLogin);
                }

                existingSite = GetExistingSite(webApp, siteModel);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing XXX");
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = existingSite,
                ObjectType = typeof(SPSite),
                ObjectDefinition = siteModel,
                ModelHost = webAppModelHost
            });


        }

        #endregion
    }
}
