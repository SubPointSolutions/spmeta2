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

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var web = folderModelHost.CurrentList.ParentWeb;
            var folder = folderModelHost.CurrentListFolder;

            var currentPage = GetWikiPageFile(web, folder, wikiPageModel);

            var context = folder.Context;

            if (typeof(WebPartDefinitionBase).IsAssignableFrom(childModelType)
                    || childModelType == typeof(DeleteWebPartsDefinition))
            {
                var listItemHost = ModelHostBase.Inherit<ListItemModelHost>(folderModelHost, itemHost =>
                {
                    itemHost.HostFolder = folderModelHost.CurrentListFolder;
                    itemHost.HostListItem = folderModelHost.CurrentListItem;
                    itemHost.HostFile = currentPage;
                    itemHost.HostList = folderModelHost.CurrentList;
                });

                action(listItemHost);
            }
            else if (
                typeof(BreakRoleInheritanceDefinition).IsAssignableFrom(childModelType) ||
                typeof(SecurityGroupLinkDefinition).IsAssignableFrom(childModelType))
            {
                var listItemHost = ModelHostBase.Inherit<ListItemModelHost>(folderModelHost, itemHost =>
                {
                    var currentListItem = currentPage.ListItemAllFields;
                    context.Load(currentListItem);
                    context.ExecuteQueryWithTrace();

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
            return GetSafeWikiPageFileName(wikiPageModel.FileName);
        }

        protected string GetSafeWikiPageFileName(string fileName)
        {
            var pageName = fileName;
            if (!pageName.EndsWith(".aspx")) pageName += ".aspx";

            return pageName;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentListFolder;

            DeployWikiPage(folderModelHost.CurrentList.ParentWeb, folderModelHost.CurrentList, folder, wikiPageModel);
        }

        private void DeployWikiPage(Web web, List list, Folder folder, WikiPageDefinition definition)
        {
            var context = folder.Context;

            var newWikiPageUrl = string.Empty;

            var contentTypeId = string.Empty;

            // pre load content type
            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                contentTypeId = definition.ContentTypeId;

            }
            else if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                contentTypeId = ContentTypeLookupService
                                            .LookupContentTypeByName(list, definition.ContentTypeName)
                                            .Id.ToString();
            }

            var file = GetWikiPageFile(web, folder, definition, out newWikiPageUrl);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = file,
                ObjectType = typeof(File),
                ObjectDefinition = definition,
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

                FieldLookupService.EnsureDefaultValues(currentListItem, definition.DefaultValues);

                if (!string.IsNullOrEmpty(contentTypeId))
                    currentListItem[BuiltInInternalFieldNames.ContentTypeId] = contentTypeId;

                currentListItem[BuiltInInternalFieldNames.WikiField] = definition.Content ?? String.Empty;

                FieldLookupService.EnsureValues(currentListItem, definition.Values, true);
                
                currentListItem.Update();

                context.ExecuteQueryWithTrace();

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newPageFile,
                    ObjectType = typeof(File),
                    ObjectDefinition = definition,
                    ModelHost = folder
                });

                context.ExecuteQueryWithTrace();
            }
            else
            {
                // TODO,override if force
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing wiki page");

                if (definition.NeedOverride)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "NeedOverride = true. Updating wiki page content.");

                    var currentListItem = file.ListItemAllFields;
                    context.Load(currentListItem);
                    context.ExecuteQueryWithTrace();

                    FieldLookupService.EnsureDefaultValues(currentListItem, definition.DefaultValues);

                    if (!string.IsNullOrEmpty(contentTypeId))
                        currentListItem[BuiltInInternalFieldNames.ContentTypeId] = contentTypeId;

                    currentListItem[BuiltInInternalFieldNames.WikiField] = definition.Content ?? String.Empty;
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
                    ObjectDefinition = definition,
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
