using System;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Services;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class PublishingPageLayoutModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PublishingPageLayoutDefinition); }
        }

        #endregion

        #region methods
        protected string GetSafePageFileName(PageDefinitionBase page)
        {
            var fileName = page.FileName;
            if (!fileName.EndsWith(".aspx")) fileName += ".aspx";

            return fileName;
        }


        protected File GetCurrentPage(List list, Folder folder, string pageName)
        {
            var item = SearchItemByName(list, folder, pageName);

            if (item != null)
                return item.File;

            return null;
        }

        protected ListItem SearchItemByName(List list, Folder folder, string pageName)
        {
            var context = list.Context;

            if (folder != null)
            {
                if (!folder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    folder.Context.Load(folder, f => f.ServerRelativeUrl);
                    folder.Context.ExecuteQueryWithTrace();
                }
            }

            var dQuery = new CamlQuery();

            string QueryString = "<View><Query><Where>" +
                             "<Eq>" +
                               "<FieldRef Name=\"FileLeafRef\"/>" +
                                "<Value Type=\"Text\">" + pageName + "</Value>" +
                             "</Eq>" +
                            "</Where></Query></View>";

            dQuery.ViewXml = QueryString;

            if (folder != null)
                dQuery.FolderServerRelativeUrl = folder.ServerRelativeUrl;

            var collListItems = list.GetItems(dQuery);

            context.Load(collListItems);
            context.ExecuteQueryWithTrace();

            return collListItems.FirstOrDefault();

        }

        protected ListItem FindPublishingPage(List list, Folder folder, PublishingPageLayoutDefinition definition)
        {
            var pageName = GetSafePageFileName(definition);
            var file = GetCurrentPage(list, folder, pageName);

            if (file != null)
                return file.ListItemAllFields;

            return null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var csomModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PublishingPageLayoutDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentListFolder;
            var list = folderModelHost.CurrentList;

            ContentType siteContentType = null;

            var contentTypeId = string.Empty;

            // pre load content type
            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                contentTypeId = definition.ContentTypeId;

            }
            else if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                contentTypeId = ContentTypeLookupService
                                            .LookupContentTypeByName(folderModelHost.CurrentList, definition.ContentTypeName)
                                            .Id.ToString();
            }

            if (!string.IsNullOrEmpty(definition.AssociatedContentTypeId))
            {
                siteContentType = folderModelHost.HostSite.RootWeb.AvailableContentTypes.GetById(definition.AssociatedContentTypeId);

                folderModelHost.HostSite.Context.Load(siteContentType);
                folderModelHost.HostSite.Context.ExecuteQueryWithTrace();
            }

            var context = folder.Context;

            var pageName = GetSafePageFileName(definition);
            var currentPageFile = GetCurrentPage(list, folder, pageName);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentPageFile,
                ObjectType = typeof(File),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            ModuleFileModelHandler.WithSafeFileOperation(list, currentPageFile, f =>
            {
                var file = new FileCreationInformation();

                file.Url = pageName;
                file.Content = Encoding.UTF8.GetBytes(definition.Content);
                file.Overwrite = definition.NeedOverride;

                return folder.Files.Add(file);

            },
            newFile =>
            {
                var newFileItem = newFile.ListItemAllFields;
                context.Load(newFileItem);
                context.ExecuteQueryWithTrace();

                var site = folderModelHost.HostSite;
                var currentPageLayoutItem = FindPageLayoutItem(site, definition.FileName);


                var currentPageLayoutItemContext = currentPageLayoutItem.Context;
                var publishingFile = currentPageLayoutItem.File;

                currentPageLayoutItemContext.Load(currentPageLayoutItem);
                currentPageLayoutItemContext.Load(currentPageLayoutItem, i => i.DisplayName);
                currentPageLayoutItemContext.Load(publishingFile);

                currentPageLayoutItemContext.ExecuteQueryWithTrace();

                FieldLookupService.EnsureDefaultValues(newFileItem, definition.DefaultValues);

                if (!string.IsNullOrEmpty(definition.Title))
                    newFileItem[BuiltInInternalFieldNames.Title] = definition.Title;

                newFileItem["MasterPageDescription"] = definition.Description;

                newFileItem[BuiltInInternalFieldNames.ContentTypeId] = BuiltInPublishingContentTypeId.PageLayout;

                // custom?
                if (!string.IsNullOrEmpty(contentTypeId))
                    newFileItem[BuiltInInternalFieldNames.ContentTypeId] = contentTypeId;

                if (!string.IsNullOrEmpty(definition.PreviewImageUrl))
                {
                    var urlValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = definition.PreviewImageUrl,
                        Context = csomModelHost
                    }).Value;

                    var urlFieldValue = new FieldUrlValue { Url = urlValue };

                    if (!string.IsNullOrEmpty(definition.PreviewImageDescription))
                        urlFieldValue.Description = definition.PreviewImageDescription;

                    newFileItem["PublishingPreviewImage"] = urlFieldValue;
                }

                if (siteContentType != null)
                {
                    newFileItem["PublishingAssociatedContentType"] = String.Format(";#{0};#{1};#", siteContentType.Name, siteContentType.Id.ToString());
                }

                FieldLookupService.EnsureValues(newFileItem, definition.Values, true);
                newFileItem.Update();

                context.ExecuteQueryWithTrace();
            });

            currentPageFile = GetCurrentPage(folderModelHost.CurrentList, folder, pageName);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentPageFile,
                ObjectType = typeof(File),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            context.ExecuteQueryWithTrace();
        }

        private ListItem FindPageLayoutItem(Site site, string pageLayoutFileName)
        {
            var rootWeb = site.RootWeb;
            var layoutsList = rootWeb.GetCatalog((int)ListTemplateType.MasterPageCatalog);

            var layoutItem = SearchItemByName(layoutsList, layoutsList.RootFolder, pageLayoutFileName);

            return layoutItem;
        }

        #endregion
    }
}
