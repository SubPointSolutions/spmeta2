using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using System.Text;
using System.Web.UI.WebControls.WebParts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WikiPageModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(WikiPageDefinition); }
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var wikiPageModel = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;

            var targetPage = FindWikiPageItem(folder, wikiPageModel);

            ModuleFileModelHandler.WithSafeFileOperation(folderModelHost.CurrentLibrary, folder,
                targetPage.Url,
                GetWikiPageName(wikiPageModel),
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

                        // targetPage.Update();
                    }
                });
        }

        protected string GetWikiPageName(WikiPageDefinition wikiPageModel)
        {
            var pageName = wikiPageModel.FileName;
            if (!pageName.EndsWith(".aspx")) pageName += ".aspx";

            return pageName;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WikiPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var list = folderModelHost.CurrentLibrary;

            //if (!string.IsNullOrEmpty(wikiPageModel.FolderUrl))
            //    throw new Exception("FolderUrl property is not supported yet!");

            var pageItem = FindWikiPageItem(folder, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = pageItem == null ? null : pageItem.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            if (pageItem == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new wiki page");

                var newWikiPageUrl = GetSafeWikiPageUrl(folder, definition);
                var newpage = folder.Files.Add(newWikiPageUrl, SPTemplateFileType.WikiPage);

                FieldLookupService.EnsureDefaultValues(newpage.ListItemAllFields, definition.DefaultValues);

                if (!string.IsNullOrEmpty(definition.ContentTypeId) ||
                   !string.IsNullOrEmpty(definition.ContentTypeName))
                {
                    if (!string.IsNullOrEmpty(definition.ContentTypeId))
                        newpage.ListItemAllFields["ContentTypeId"] = ContentTypeLookupService.LookupListContentTypeById(list, definition.ContentTypeId);

                    if (!string.IsNullOrEmpty(definition.ContentTypeName))
                        newpage.ListItemAllFields["ContentTypeId"] = ContentTypeLookupService.LookupContentTypeByName(list, definition.ContentTypeName);
                }

                newpage.ListItemAllFields[SPBuiltInFieldId.WikiField] = definition.Content ?? string.Empty;

                FieldLookupService.EnsureValues(newpage.ListItemAllFields, definition.Values, true);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newpage,
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });

                newpage.ListItemAllFields.Update();
                newpage.Update();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing wiki page");

                if (definition.NeedOverride)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "NeedOverride = true. Updating wiki page content.");

                    FieldLookupService.EnsureDefaultValues(pageItem, definition.DefaultValues);

                    if (!string.IsNullOrEmpty(definition.ContentTypeId) ||
                        !string.IsNullOrEmpty(definition.ContentTypeName))
                    {
                        if (!string.IsNullOrEmpty(definition.ContentTypeId))
                            pageItem["ContentTypeId"] = ContentTypeLookupService.LookupListContentTypeById(list, definition.ContentTypeId);

                        if (!string.IsNullOrEmpty(definition.ContentTypeName))
                            pageItem["ContentTypeId"] = ContentTypeLookupService.LookupContentTypeByName(list, definition.ContentTypeName);
                    }

                    pageItem[SPBuiltInFieldId.WikiField] = definition.Content ?? string.Empty;

                    FieldLookupService.EnsureValues(pageItem, definition.Values, true);
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
                    Object = pageItem.File,
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });

                pageItem.Update();
                pageItem.File.Update();
            }
        }

        protected string GetSafeWikiPageUrl(SPFolder folder, WikiPageDefinition wikiPageModel)
        {
            // TODO, fix +/ to SPUtility
            return folder.ServerRelativeUrl + "/" + GetSafeWikiPageFileName(wikiPageModel);
        }

        protected string GetSafeWikiPageFileName(WikiPageDefinition wikiPageModel)
        {
            return GetSafeWikiPageFileName(wikiPageModel.FileName);
        }

        protected string GetSafeWikiPageFileName(string wikiPageName)
        {
            if (!wikiPageName.EndsWith(".aspx"))
                wikiPageName += ".aspx";

            return wikiPageName;
        }

        protected SPListItem FindWikiPageItem(SPFolder folder, WikiPageDefinition wikiPageModel)
        {
            var file = folder.ParentWeb.GetFile(GetSafeWikiPageUrl(folder, wikiPageModel));

            return file.Exists ? file.Item : null;
        }

        #endregion
    }
}
