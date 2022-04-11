﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using SPMeta2.Services;

namespace SPMeta2.CSOM.ModelHandlers.Base
{
    public abstract class MasterPageModelHandlerBase : CSOMModelHandlerBase
    {
        #region properties

        public abstract string PageContentTypeId { get; set; }
        public abstract string PageFileExtension { get; set; }

        #endregion

        #region methods
        protected string GetSafePageFileName(PageDefinitionBase page)
        {
            var fileName = page.FileName;

            if (!fileName.ToLower().EndsWith(PageFileExtension.ToLower()))
            {
                fileName += PageFileExtension;
            }

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

        protected File FindPageFile(List list, Folder folder, MasterPageDefinitionBase definition)
        {
            var pageName = GetSafePageFileName(definition);
            return GetCurrentPage(list, folder, pageName);
        }

        protected ListItem FindPage(List list, Folder folder, MasterPageDefinitionBase definition)
        {
            var pageName = GetSafePageFileName(definition);

            return FindPageByName(list, folder, pageName);
        }

        protected ListItem FindPageByName(List list, Folder folder, string pageName)
        {
            var file = GetCurrentPage(list, folder, pageName);

            if (file != null)
                return file.ListItemAllFields;

            return null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<MasterPageDefinitionBase>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentListFolder;
            var list = folderModelHost.CurrentList;

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
                file.Content = definition.Content;
                file.Overwrite = definition.NeedOverride;

                return folder.Files.Add(file);

            },
            newFile =>
            {
                var newFileItem = newFile.ListItemAllFields;
                context.Load(newFileItem);
                context.ExecuteQueryWithTrace();

                //var site = folderModelHost.HostSite;
                //var currentPageLayoutItem = FindPageLayoutItem(site, definition.FileName);

                //var currentPageLayoutItemContext = currentPageLayoutItem.Context;
                //var publishingFile = currentPageLayoutItem.File;

                //currentPageLayoutItemContext.Load(currentPageLayoutItem);
                //currentPageLayoutItemContext.Load(currentPageLayoutItem, i => i.DisplayName);
                //currentPageLayoutItemContext.Load(publishingFile);

                //currentPageLayoutItemContext.ExecuteQueryWithTrace();

                // ** SIC.. found with Problem with url in MasterPageSettings #936
                // https://github.com/SubPointSolutions/spmeta2/issues/936

                // * /_catalogs/masterpage - would have 'Title' field (and correct content types)
                // * /my-sub-web/_catalogs/masterpage - would NOT have 'Title' fiels so that provision fails

                // so performing Title update only for the root web
                if (folderModelHost.HostSite.ServerRelativeUrl == folderModelHost.HostWeb.ServerRelativeUrl)
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Updating master page 'Title' on the root web.", null);
                    newFileItem[BuiltInInternalFieldNames.Title] = definition.Title;
                }
                else
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Skipping master page 'Title' update. Subweb is detcted.", null);
                }

                newFileItem["MasterPageDescription"] = definition.Description;
                newFileItem[BuiltInInternalFieldNames.ContentTypeId] = PageContentTypeId;

                if (definition.UIVersion.Count > 0)
                {
                    newFileItem["UIVersion"] = string.Join(";#", definition.UIVersion.ToArray());
                }

                newFileItem["DefaultCssFile"] = definition.DefaultCSSFile;

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
