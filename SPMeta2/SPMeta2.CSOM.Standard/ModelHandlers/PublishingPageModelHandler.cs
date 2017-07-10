using System;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Exceptions;

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

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var folderModelHost = modelHost as FolderModelHost;
            var definition = model as PublishingPageDefinition;

            Folder folder = folderModelHost.CurrentListFolder;

            if (folder != null && definition != null)
            {
                var context = folder.Context;
                var currentPage = GetCurrentPage(folderModelHost.CurrentList, folder, GetSafePageFileName(definition));

                if (currentPage == null)
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find publishing page fo definition:[{0}]",
                        definition));
                }

                if (typeof(WebPartDefinitionBase).IsAssignableFrom(childModelType)
                    || childModelType == typeof(DeleteWebPartsDefinition))
                {
                    var listItemHost = ModelHostBase.Inherit<ListItemModelHost>(folderModelHost, itemHost =>
                    {
                        itemHost.HostFile = currentPage;
                        itemHost.HostList = folderModelHost.CurrentList;
                    });

                    action(listItemHost);

                    //currentListItem.Update();
                }
                else if (typeof(BreakRoleInheritanceDefinition).IsAssignableFrom(childModelType)
                        || typeof(SecurityGroupLinkDefinition).IsAssignableFrom(childModelType))
                {
                    var currentListItem = currentPage.ListItemAllFields;
                    context.Load(currentListItem);
                    context.ExecuteQueryWithTrace();

                    var listItemHost = ModelHostBase.Inherit<ListItemModelHost>(folderModelHost, itemHost =>
                    {

                        itemHost.HostListItem = currentListItem;
                    });

                    action(listItemHost);
                }
                else
                {
                    action(currentPage);
                }

                //context.ExecuteQueryWithTrace();
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

            var folder = folderModelHost.CurrentListFolder;
            var list = folderModelHost.CurrentList;

            var definition = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

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
                // create if only file does not exist
                // overriting spoils the fields's content
                // Investigate publishing page provision, Content property #744
                // https://github.com/SubPointSolutions/spmeta2/issues/744

                if (f == null || !f.Exists)
                {
                    var file = new FileCreationInformation();
                    var pageContent = PublishingPageTemplates.RedirectionPageMarkup;

                    file.Url = pageName;
                    file.Content = Encoding.UTF8.GetBytes(pageContent);
                    file.Overwrite = definition.NeedOverride;

                    return folder.Files.Add(file);
                }

                return f;
            },
            newFile =>
            {
                var newFileItem = newFile.ListItemAllFields;
                context.Load(newFileItem);
                context.ExecuteQueryWithTrace();

                var site = folderModelHost.HostSite;
                var currentPageLayoutItem = FindPageLayoutItem(site, definition.PageLayoutFileName);

                var currentPageLayoutItemContext = currentPageLayoutItem.Context;
                var publishingFile = currentPageLayoutItem.File;

                currentPageLayoutItemContext.Load(currentPageLayoutItem);
                currentPageLayoutItemContext.Load(currentPageLayoutItem, i => i.DisplayName);
                currentPageLayoutItemContext.Load(publishingFile);

                currentPageLayoutItemContext.ExecuteQueryWithTrace();

                // settig up dfault values if there is PublishingPageLayout setup
                FieldLookupService.EnsureDefaultValues(newFileItem, definition.DefaultValues);

                if (!string.IsNullOrEmpty(definition.Title))
                    newFileItem[BuiltInInternalFieldNames.Title] = definition.Title;

                if (!string.IsNullOrEmpty(definition.Description))
                    newFileItem[BuiltInInternalFieldNames.Comments] = definition.Description;

                if (!string.IsNullOrEmpty(definition.Content))
                    newFileItem[BuiltInInternalPublishingFieldNames.PublishingPageContent] = definition.Content;

                newFileItem[BuiltInInternalFieldNames.PublishingPageLayout] = publishingFile.ServerRelativeUrl + ", " + currentPageLayoutItem.DisplayName;

                var associatedContentTypeStringValue = ConvertUtils.ToString(currentPageLayoutItem[BuiltInInternalFieldNames.PublishingAssociatedContentType]);

                if (!string.IsNullOrEmpty(associatedContentTypeStringValue))
                {
                    var contentTypeValues = associatedContentTypeStringValue.Split(new string[] { ";#" }, StringSplitOptions.None);
                    var associatedContentTypeName = contentTypeValues[1];
                    var associatedContentTypeId = contentTypeValues[2];

                    newFileItem[BuiltInInternalFieldNames.ContentTypeId] = associatedContentTypeId;
                }

                if (!string.IsNullOrEmpty(contentTypeId))
                    newFileItem[BuiltInInternalFieldNames.ContentTypeId] = contentTypeId;

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
