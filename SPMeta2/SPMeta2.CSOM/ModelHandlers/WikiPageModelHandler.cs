using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
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
            context.ExecuteQueryWithTrace();

            if (typeof(WebPartDefinitionBase).IsAssignableFrom(childModelType))
            {
                var listItemHost = ModelHostBase.Inherit<ListItemModelHost>(folderModelHost, itemHost =>
                {
                    itemHost.HostListItem = currentListItem;
                });

                action(listItemHost);
            }
            else if (
                typeof(BreakRoleInheritanceDefinition).IsAssignableFrom(childModelType) ||
                typeof(SecurityGroupLinkDefinition).IsAssignableFrom(childModelType))
            {
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

            context.ExecuteQueryWithTrace();
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
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new wiki page");

                var newPageFile = folder.Files.AddTemplateFile(newWikiPageUrl, TemplateFileType.WikiPage);

                context.Load(newPageFile);

                var currentListItem = newPageFile.ListItemAllFields;
                context.Load(currentListItem);
                context.ExecuteQueryWithTrace();

                currentListItem[BuiltInInternalFieldNames.WikiField] = wikiPageModel.Content ?? String.Empty;
                currentListItem.Update();

                context.ExecuteQueryWithTrace();

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

                context.ExecuteQueryWithTrace();
            }
            else
            {
                // TODO,override if force
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing wiki page");

                if (wikiPageModel.NeedOverride)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "NeedOverride = true. Updating wiki page content.");

                    var currentListItem = file.ListItemAllFields;
                    context.Load(currentListItem);
                    context.ExecuteQueryWithTrace();

                    currentListItem[BuiltInInternalFieldNames.WikiField] = wikiPageModel.Content ?? String.Empty;
                    currentListItem.Update();
                }
                else
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "NeedOverride = false. Skipping Updating wiki page content.");
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

                context.ExecuteQueryWithTrace();
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
            context.ExecuteQueryWithTrace();

            newWikiPageUrl = folder.ServerRelativeUrl + "/" + pageName;

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving file with URL: [{0}]", newWikiPageUrl);
            var file = web.GetFileByServerRelativeUrl(newWikiPageUrl);

            context.Load(file, f => f.Exists);
            context.ExecuteQueryWithTrace();

            if (file.Exists)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Returning existing file");
                return file;
            }

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "File does not exist. Returning NULL");

            return null;
        }

        #endregion
    }
}
