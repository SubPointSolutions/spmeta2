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
using SPMeta2.Services;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class PublishingPageLayoutModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PublishingPageLayoutDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;
            var publishingPageModel = model.WithAssertAndCast<PublishingPageLayoutDefinition>("model", value => value.RequireNotNull());

            DeployPublishingPage(modelHost, listModelHost.CurrentLibrary, folder, publishingPageModel);
        }

        private void DeployPublishingPage(object modelHost, SPList list, SPFolder folder, PublishingPageLayoutDefinition definition)
        {
            var web = list.ParentWeb;
            var targetPage = GetCurrentObject(folder, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = targetPage == null ? null : targetPage.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (targetPage == null)
                targetPage = CreateObject(modelHost, folder, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = targetPage.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            ModuleFileModelHandler.WithSafeFileOperation(list, folder,
                targetPage.Url,
                GetSafePageFileName(definition),
                Encoding.UTF8.GetBytes(definition.Content),
                definition.NeedOverride,
                null,
                afterFile =>
                {
                    //var pageItem = afterFile.Properties;
                    var pageItem = afterFile.ListItemAllFields;

                    FieldLookupService.EnsureDefaultValues(pageItem, definition.DefaultValues);

                    if (!string.IsNullOrEmpty(definition.Title))
                    {
                        //pageItem["vti_title"] = definition.Title;
                        pageItem["Title"] = definition.Title;
                    }

                    // ootb ?
                    pageItem[BuiltInInternalFieldNames.ContentTypeId] = BuiltInPublishingContentTypeId.PageLayout;


                    // custom?
                    if (!string.IsNullOrEmpty(definition.ContentTypeId) ||
                       !string.IsNullOrEmpty(definition.ContentTypeName))
                    {
                        if (!string.IsNullOrEmpty(definition.ContentTypeId))
                            pageItem["ContentTypeId"] = ContentTypeLookupService.LookupListContentTypeById(list, definition.ContentTypeId);

                        if (!string.IsNullOrEmpty(definition.ContentTypeName))
                            pageItem["ContentTypeId"] = ContentTypeLookupService.LookupContentTypeByName(list, definition.ContentTypeName);
                    }

                    if (!string.IsNullOrEmpty(definition.Description))
                    {
                        // did not work
                        pageItem["MasterPageDescription"] = definition.Description;
                        //pageItem.Properties["MasterPageDescription"] = definition.Description;
                    }


                    if (!string.IsNullOrEmpty(definition.PreviewImageUrl))
                    {
                        var urlValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                        {
                            Value = definition.PreviewImageUrl,
                            Context = web
                        }).Value;

                        var urlFieldValue = new SPFieldUrlValue { Url = urlValue };

                        if (!string.IsNullOrEmpty(definition.PreviewImageDescription))
                            urlFieldValue.Description = definition.PreviewImageDescription;

                        pageItem["PublishingPreviewImage"] = urlFieldValue.ToString();
                    }

                    if (!string.IsNullOrEmpty(definition.AssociatedContentTypeId))
                    {
                        var siteContentType = web.AvailableContentTypes[new SPContentTypeId(definition.AssociatedContentTypeId)];

                        pageItem["PublishingAssociatedContentType"] = String.Format(";#{0};#{1};#",
                            siteContentType.Name,
                            siteContentType.Id.ToString());
                    }

                    FieldLookupService.EnsureValues(pageItem, definition.Values, true);


                    pageItem.Update();
                });
        }

        private SPListItem CreateObject(object modelHost, SPFolder folder, PublishingPageLayoutDefinition definition)
        {
            var pageName = GetSafePageFileName(definition);
            var fileContent = Encoding.UTF8.GetBytes(definition.Content);

            ModuleFileModelHandler.DeployModuleFile(folder,
                  SPUrlUtility.CombineUrl(folder.ServerRelativeUrl, pageName),
                  pageName,
                  fileContent,
                  true,
                  null,
                  null);

            return GetCurrentObject(folder, definition);
        }

        protected string GetSafePageFileName(PublishingPageLayoutDefinition pageModel)
        {
            var pageName = pageModel.FileName;

            if (!pageName.EndsWith(".aspx"))
                pageName += ".aspx";

            return pageName;
        }

        //public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        //{
        //    var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

        //    var folder = listModelHost.CurrentLibraryFolder;
        //    var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

        //    var targetPage = FindPublishingPage(folder, publishingPageModel);

        //    ModuleFileModelHandler.WithSafeFileOperation(listModelHost.CurrentLibrary, folder,
        //        targetPage.Url,
        //        GetSafePageFileName(publishingPageModel),
        //        Encoding.UTF8.GetBytes(PublishingPageTemplates.RedirectionPageMarkup),
        //        false,
        //        null,
        //        afterFile =>
        //        {
        //            using (var webPartManager = afterFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
        //            {
        //                var webpartPageHost = new WebpartPageModelHost
        //                {
        //                    HostFile = afterFile,
        //                    PageListItem = targetPage,
        //                    SPLimitedWebPartManager = webPartManager
        //                };

        //                action(webpartPageHost);
        //            }
        //        });
        //}

        protected SPListItem GetCurrentObject(SPFolder folder, PublishingPageLayoutDefinition definition)
        {
            var pageLayoutName = GetSafePageFileName(definition);

            foreach (SPFile file in folder.Files)
                if (file.Name.ToUpper() == pageLayoutName.ToUpper())
                    return file.Item;

            return null;
        }

        #endregion
    }
}
