﻿using System;
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

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var folderModelHost = modelHost as FolderModelHost;
            var pageDefinition = model as PublishingPageDefinition;

            Folder folder = folderModelHost.CurrentListFolder;

            if (folder != null && pageDefinition != null)
            {
                var context = folder.Context;
                var currentPage = GetCurrentPage(folderModelHost.CurrentList, folder, GetSafePageFileName(pageDefinition));

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

            var publishingPageModel = model.WithAssertAndCast<PublishingPageDefinition>("model", value => value.RequireNotNull());

            var context = folder.Context;

            var stringCustomContentType = string.Empty;
            // preload custm content type
            if (!string.IsNullOrEmpty(publishingPageModel.ContentTypeName))
            {
                var listContentTypes = list.ContentTypes;
                context.Load(listContentTypes);
                context.ExecuteQueryWithTrace();

                var listContentType = listContentTypes.ToList()
                                                      .FirstOrDefault(c => c.Name.ToUpper() == publishingPageModel.ContentTypeName.ToUpper());

                if (listContentType == null)
                {
                    throw new ArgumentNullException(
                        string.Format("Cannot find content type with Name:[{0}] in List:[{1}]",
                            new string[]
                                    {
                                        publishingPageModel.ContentTypeName,
                                        list.Title
                                    }));
                }

                stringCustomContentType = listContentType.Id.ToString();
            }

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
                context.ExecuteQueryWithTrace();

                var site = folderModelHost.HostSite;
                var currentPageLayoutItem = FindPageLayoutItem(site, publishingPageModel.PageLayoutFileName);

                var currentPageLayoutItemContext = currentPageLayoutItem.Context;
                var publishingFile = currentPageLayoutItem.File;

                currentPageLayoutItemContext.Load(currentPageLayoutItem);
                currentPageLayoutItemContext.Load(currentPageLayoutItem, i => i.DisplayName);
                currentPageLayoutItemContext.Load(publishingFile);

                currentPageLayoutItemContext.ExecuteQueryWithTrace();

                // settig up dfault values if there is PublishingPageLayout setup
                EnsureDefaultValues(newFileItem, publishingPageModel);

                if (!string.IsNullOrEmpty(publishingPageModel.Title))
                    newFileItem[BuiltInInternalFieldNames.Title] = publishingPageModel.Title;

                if (!string.IsNullOrEmpty(publishingPageModel.Description))
                    newFileItem[BuiltInInternalFieldNames.Comments] = publishingPageModel.Description;

                newFileItem[BuiltInInternalFieldNames.PublishingPageLayout] = publishingFile.ServerRelativeUrl + ", " + currentPageLayoutItem.DisplayName;

                var contentTypeStringValue = ConvertUtils.ToString(currentPageLayoutItem[BuiltInInternalFieldNames.PublishingAssociatedContentType]);

                if (!string.IsNullOrEmpty(contentTypeStringValue))
                {
                    var contentTypeValues = contentTypeStringValue.Split(new string[] { ";#" }, StringSplitOptions.None);
                    var contentTypeName = contentTypeValues[1];
                    var contentTypeId = contentTypeValues[2];

                    newFileItem[BuiltInInternalFieldNames.ContentTypeId] = contentTypeId;
                }

                if (!string.IsNullOrEmpty(stringCustomContentType))
                    newFileItem[BuiltInInternalFieldNames.ContentTypeId] = stringCustomContentType;

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
                ObjectDefinition = publishingPageModel,
                ModelHost = modelHost
            });

            context.ExecuteQueryWithTrace();
        }

        private static void EnsureDefaultValues(ListItem newFileItem, PublishingPageDefinition publishingPageModel)
        {
            foreach (var defaultValue in publishingPageModel.DefaultValues)
            {
                if (!string.IsNullOrEmpty(defaultValue.FieldName))
                {
                    if (newFileItem.FieldValues.ContainsKey(defaultValue.FieldName))
                    {
                        if (newFileItem[defaultValue.FieldName] == null)
                            newFileItem[defaultValue.FieldName] = defaultValue.Value;
                    }
                }
                else if (defaultValue.FieldId.HasValue && defaultValue.FieldId != default(Guid))
                {
                    // unsupported by CSOM API yet
                }
            }
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
