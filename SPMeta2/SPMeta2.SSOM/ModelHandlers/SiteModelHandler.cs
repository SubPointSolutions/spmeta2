using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;

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

        private void DeploySite(WebApplicationModelHost webAppModelHost, Microsoft.SharePoint.Administration.SPWebApplication webApp, SiteDefinition siteModel)
        {
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
            }
        }

        #endregion
    }
}
