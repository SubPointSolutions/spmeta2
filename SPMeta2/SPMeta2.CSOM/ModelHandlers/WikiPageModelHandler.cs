using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WikiPageModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WikiPageDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var web = folderModelHost.CurrentList.ParentWeb;
            var folder = folderModelHost.CurrentLibraryFolder;

            var currentPage = GetWikiPageFile(web, folder, wikiPageModel);

            var context = folder.Context;

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

            context.ExecuteQuery();
        }

        protected string GetSafeWikiPageFileName(WikiPageDefinition wikiPageModel)
        {
            var pageName = wikiPageModel.FileName;
            if (!pageName.EndsWith(".aspx")) pageName += ".aspx";

            return pageName;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;

            DeployWikiPage(folderModelHost.CurrentList.ParentWeb, folder, wikiPageModel);
        }

        private void DeployWikiPage(Web web, Folder folder, WikiPageDefinition wikiPageModel)
        {
            var context = folder.Context;

            var newWikiPageUrl = string.Empty;
            var file = GetWikiPageFile(web, folder, wikiPageModel, out newWikiPageUrl);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = file,
                ObjectType = typeof(File),
                ObjectDefinition = wikiPageModel,
                ModelHost = folder
            });

            if (file == null)
            {
                var newPageFile = folder.Files.AddTemplateFile(newWikiPageUrl, TemplateFileType.WikiPage);

                //newPageFile.ListItemAllFields

                context.Load(newPageFile);

                var currentListItem = newPageFile.ListItemAllFields;
                context.Load(currentListItem);
                context.ExecuteQuery();

                currentListItem[BuiltInInternalFieldNames.WikiField] = wikiPageModel.Content ?? String.Empty;
                currentListItem.Update();

                context.ExecuteQuery();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newPageFile,
                    ObjectType = typeof(File),
                    ObjectDefinition = wikiPageModel,
                    ModelHost = folder
                });

                context.ExecuteQuery();

            }
            else
            {
                // TODO,override if force

                if (wikiPageModel.NeedOverride)
                {
                    var currentListItem = file.ListItemAllFields;
                    context.Load(currentListItem);
                    context.ExecuteQuery();

                    currentListItem[BuiltInInternalFieldNames.WikiField] = wikiPageModel.Content ?? String.Empty;
                    currentListItem.Update();
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = file,
                    ObjectType = typeof(File),
                    ObjectDefinition = wikiPageModel,
                    ModelHost = folder
                });

                context.ExecuteQuery();
            }
        }

        protected File GetWikiPageFile(Web web, Folder folder, WikiPageDefinition wikiPageModel)
        {
            var newWikiPageUrl = string.Empty;
            var result = GetWikiPageFile(web, folder, wikiPageModel, out newWikiPageUrl);

            return result;
        }

        protected File GetWikiPageFile(Web web, Folder folder, WikiPageDefinition wikiPageModel, out string newWikiPageUrl)
        {
            var context = folder.Context;

            //if (!string.IsNullOrEmpty(wikiPageModel.FolderUrl))
            //    throw new Exception("FolderUrl property is not supported yet!");

            var pageName = GetSafeWikiPageFileName(wikiPageModel);

            context.Load(folder, l => l.ServerRelativeUrl);
            context.ExecuteQuery();

            newWikiPageUrl = folder.ServerRelativeUrl + "/" + pageName;
            var file = web.GetFileByServerRelativeUrl(newWikiPageUrl);

            context.Load(file, f => f.Exists);
            context.ExecuteQuery();

            if (file.Exists)
                return file;

            return null;
        }

        #endregion
    }
}
