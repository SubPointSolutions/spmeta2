using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class PublishingPageModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PublishingPageDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;
            var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            DeployPublishingPage(modelHost, listModelHost.CurrentLibrary, folder, publishingPageModel);
        }

        internal static readonly DateTime NeverEndDate = new DateTime(2050, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal static readonly DateTime ImmediateDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private void DeployPublishingPage(object modelHost, SPList list, SPFolder folder, PublishingPageDefinition publishingPageModel)
        {
            var targetPage = FindPublishingPage(folder, publishingPageModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = targetPage == null ? null : targetPage.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = publishingPageModel,
                ModelHost = modelHost
            });

            if (targetPage == null)
                targetPage = CreatePublishingPage(modelHost, folder, publishingPageModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = targetPage.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = publishingPageModel,
                ModelHost = modelHost
            });

            ModuleFileModelHandler.WithSafeFileOperation(list, folder,
                targetPage.Url,
                GetSafePublishingPageFileName(publishingPageModel),
                Encoding.UTF8.GetBytes(PublishingPageTemplates.RedirectionPageMarkup),
                false,
                null,
                afterFile =>
                {
                    var web = list.ParentWeb;
                    var currentPageLayoutItem = FindPageLayoutItem(web, publishingPageModel.PageLayoutFileName);

                    var pageItem = afterFile.Item;

                    pageItem[BuiltInFieldId.Title] = publishingPageModel.Title;
                    pageItem[BuiltInPublishingFieldId.Description] = publishingPageModel.Description;

                    pageItem[BuiltInPublishingFieldId.ExpiryDate] = NeverEndDate;
                    pageItem[BuiltInPublishingFieldId.StartDate] = ImmediateDate;

                    pageItem[BuiltInPublishingFieldId.Contact] = list.ParentWeb.CurrentUser;

                    pageItem[BuiltInPublishingFieldId.PublishingPageContent] = publishingPageModel.Content;

                    pageItem[BuiltInFieldId.ContentTypeId] = currentPageLayoutItem[BuiltInPublishingFieldId.AssociatedContentType];
                    pageItem[BuiltInPublishingFieldId.PageLayout] = new SPFieldUrlValue()
                    {
                        Url = currentPageLayoutItem.File.ServerRelativeUrl,
                        Description = currentPageLayoutItem.Title
                    };
                    pageItem.Properties["PublishingPageLayoutName"] = currentPageLayoutItem.Name;


                    pageItem.SystemUpdate();
                });
        }

        private SPListItem FindPageLayoutItem(SPWeb web, string pageLayoutFileName)
        {
            SPListItem currentPageLayoutItem = null;

            var pageLayoutContentType = BuiltInPublishingContentTypeId.PageLayout.ToUpper();


            var rootWeb = web.Site.RootWeb;
            var layoutsList = rootWeb.GetCatalog(SPListTemplateType.MasterPageCatalog);

            // TODO, performance
            var pageLayouts = layoutsList.Items.OfType<SPListItem>()
                                                 .Where(i => i.ContentTypeId.ToString().ToUpper().StartsWith(pageLayoutContentType));

            foreach (var pageLayout in pageLayouts)
            {
                if (pageLayout.Name.ToUpper() == pageLayoutFileName.ToUpper())
                {
                    currentPageLayoutItem = pageLayout;
                    break;
                }
            }

            return currentPageLayoutItem;
        }

        private SPListItem CreatePublishingPage(object modelHost, SPFolder folder, PublishingPageDefinition publishingPageModel)
        {
            var pageName = GetSafePublishingPageFileName(publishingPageModel);
            var fileContent = Encoding.UTF8.GetBytes(PublishingPageTemplates.RedirectionPageMarkup);

            ModuleFileModelHandler.DeployModuleFile(folder,
                  SPUrlUtility.CombineUrl(folder.ServerRelativeUrl, pageName),
                  pageName,
                  fileContent,
                  true,
                  null,
                  null);

            return FindPublishingPage(folder, publishingPageModel);
        }

        protected string GetSafePublishingPageFileName(PublishingPageDefinition pageModel)
        {
            var webPartPageName = pageModel.FileName;

            if (!webPartPageName.EndsWith(".aspx"))
                webPartPageName += ".aspx";

            return webPartPageName;
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;
            var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            var targetPage = FindPublishingPage(folder, publishingPageModel);

            ModuleFileModelHandler.WithSafeFileOperation(listModelHost.CurrentLibrary, folder,
                targetPage.Url,
                GetSafePublishingPageFileName(publishingPageModel),
                Encoding.UTF8.GetBytes(PublishingPageTemplates.RedirectionPageMarkup),
                false,
                null,
                afterFile =>
                {
                    using (var webPartManager = afterFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
                    {
                        var webpartPageHost = new WebpartPageModelHost
                        {
                            HostFile = afterFile,
                            PageListItem = targetPage,
                            SPLimitedWebPartManager = webPartManager
                        };

                        action(webpartPageHost);
                    }
                });
        }

        protected SPListItem FindPublishingPage(SPFolder folder, PublishingPageDefinition webpartPageModel)
        {
            var webPartPageName = GetSafePublishingPageFileName(webpartPageModel);

            if (!webPartPageName.EndsWith(".aspx"))
                webPartPageName += ".aspx";

            foreach (SPFile file in folder.Files)
                if (file.Name.ToUpper() == webPartPageName.ToUpper())
                    return file.Item;

            return null;
        }

        #endregion
    }
}
