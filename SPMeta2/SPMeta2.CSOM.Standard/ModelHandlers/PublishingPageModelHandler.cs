using System;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class PublishingPageModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PublishingPageDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var folderModelHost = modelHost as FolderModelHost;
            var pageDefinition = model as PublishingPageDefinition;

            Folder folder = folderModelHost.CurrentLibraryFolder;

            if (folder != null && pageDefinition != null)
            {
                var context = folder.Context;
                var currentPage = GetCurrentPage(folderModelHost.CurrentList, folder, GetSafePageFileName(pageDefinition));

                var currentListItem = currentPage.ListItemAllFields;
                context.Load(currentListItem);
                context.ExecuteQuery();

                if (typeof(WebPartDefinitionBase).IsAssignableFrom(childModelType))
                {
                    var listItemHost = ModelHostBase.Inherit<ListItemModelHost>(folderModelHost, itemHost =>
                    {
                        itemHost.HostListItem = currentListItem;
                    });

                    action(listItemHost);

                    //currentListItem.Update();
                }

                //context.ExecuteQuery();
            }
            else
            {
                action(modelHost);
            }
        }

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
                    folder.Context.ExecuteQuery();
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
            context.ExecuteQuery();

            return collListItems.FirstOrDefault();

        }

        protected ListItem FindPublishingPage(List list, Folder folder, PublishingPageDefinition definition)
        {
            var pageName = GetSafePageFileName(definition);
            var file = GetCurrentPage(list, folder, pageName);

            if (file != null)
                return file.ListItemAllFields;

            return null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var list = folderModelHost.CurrentList;

            var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            var context = folder.Context;

            var pageName = GetSafePageFileName(publishingPageModel);
            var currentPageFile = GetCurrentPage(list, folder, pageName);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentPageFile,
                ObjectType = typeof(File),
                ObjectDefinition = publishingPageModel,
                ModelHost = modelHost
            });

            ModuleFileModelHandler.WithSafeFileOperation(list, currentPageFile, f =>
            {
                var file = new FileCreationInformation();
                var pageContent = PublishingPageTemplates.RedirectionPageMarkup;

                file.Url = pageName;
                file.Content = Encoding.UTF8.GetBytes(pageContent);
                file.Overwrite = publishingPageModel.NeedOverride;

                return folder.Files.Add(file);

            },
            newFile =>
            {
                var newFileItem = newFile.ListItemAllFields;
                context.Load(newFileItem);
                context.ExecuteQuery();

                var site = folderModelHost.HostSite;
                var currentPageLayoutItem = FindPageLayoutItem(site, publishingPageModel.PageLayoutFileName);

                var currentPageLayoutItemContext = currentPageLayoutItem.Context;
                var publishingFile = currentPageLayoutItem.File;

                currentPageLayoutItemContext.Load(currentPageLayoutItem);
                currentPageLayoutItemContext.Load(currentPageLayoutItem, i => i.DisplayName);
                currentPageLayoutItemContext.Load(publishingFile);

                currentPageLayoutItemContext.ExecuteQuery();

                newFileItem["Title"] = publishingPageModel.Title;
                newFileItem["Comments"] = publishingPageModel.Description;

                newFileItem["PublishingPageLayout"] = publishingFile.ServerRelativeUrl + ", " + currentPageLayoutItem.DisplayName;
                newFileItem["ContentTypeId"] = currentPageLayoutItem["PublishingAssociatedContentType"];

                newFileItem.Update();

                context.ExecuteQuery();
            });

            currentPageFile = GetCurrentPage(folderModelHost.CurrentList, folder, pageName);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentPageFile,
                ObjectType = typeof(File),
                ObjectDefinition = publishingPageModel,
                ModelHost = modelHost
            });

            context.ExecuteQuery();
        }



        //if (item != null)
        //    return item.File;

        //return null;

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
