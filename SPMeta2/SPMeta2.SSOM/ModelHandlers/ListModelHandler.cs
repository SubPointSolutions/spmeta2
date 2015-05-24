using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ListDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var web = webModelHost.HostWeb;

            var listDefinition = model as ListDefinition;

            if (web != null && listDefinition != null)
            {
                // This is very important line ->  adding new 'fake list'
                //
                // Nintex workflow deployment web service updates the list, so that version of the list becomes +4
                // Current SPWeb has not been updated, current list will be 4 versions behind so you will have 'Save conflict' exception
                //
                // We try to add new list, so SPListCollection is invalidated.
                // Surely, we won't save this list.
                try
                {
                    var tmpListId = web.Lists.Add(Guid.NewGuid().ToString(), string.Empty, Microsoft.SharePoint.SPListTemplateType.GenericList);
                    var tmpList = web.Lists[tmpListId];
                    tmpList.Delete();
                }
                catch (Exception)
                {
                }

                var list = web.GetList(SPUtility.ConcatUrls(web.ServerRelativeUrl, listDefinition.GetListUrl()));

                var listModelHost = new ListModelHost
                {
                    HostList = list
                };

                if (childModelType == typeof(ModuleFileDefinition))
                {
                    var folderModelHost = new FolderModelHost
                    {
                        CurrentLibrary = list as SPDocumentLibrary,
                        CurrentLibraryFolder = list.RootFolder,

                        CurrentList = (list as SPDocumentLibrary != null) ? null : list,
                        CurrentListItem = null,
                    };

                    action(folderModelHost);
                }
                else if (childModelType == typeof(FolderDefinition))
                {
                    var folderModelHost = new FolderModelHost
                    {
                        CurrentLibrary = list as SPDocumentLibrary,
                        CurrentLibraryFolder = list.RootFolder,

                        CurrentList = (list as SPDocumentLibrary != null) ? null : list,
                        CurrentListItem = null,
                    };

                    action(folderModelHost);
                }
                else if (typeof(PageDefinitionBase).IsAssignableFrom(childModelType))
                {
                    var folderModelHost = new FolderModelHost
                    {
                        CurrentLibrary = list as SPDocumentLibrary,
                        CurrentLibraryFolder = list.RootFolder,

                        CurrentList = (list as SPDocumentLibrary != null) ? null : list,
                        CurrentListItem = null,
                    };

                    action(folderModelHost);
                }

                else
                {
                    action(listModelHost);
                }

                if (listModelHost.ShouldUpdateHost)
                    list.Update();
            }
            else
            {
                action(modelHost);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var web = webModelHost.HostWeb;

            var listModel = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());

            // min provision
            var targetList = GetOrCreateList(modelHost, web, listModel);

            MapListProperties(targetList, listModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = targetList,
                ObjectType = typeof(SPList),
                ObjectDefinition = listModel,
                ModelHost = modelHost
            });

            targetList.Update();
        }

        private static void MapListProperties(SPList list, ListDefinition definition)
        {
            list.Title = definition.Title;

            // SPBug, again & again, must not be null
            list.Description = definition.Description ?? string.Empty;
            list.ContentTypesEnabled = definition.ContentTypesEnabled;

            if (!string.IsNullOrEmpty(definition.DraftVersionVisibility))
            {
                var draftOption = (DraftVisibilityType)Enum.Parse(typeof(DraftVisibilityType), definition.DraftVersionVisibility);
                list.DraftVersionVisibility = draftOption;
            }

            // IRM
            if (definition.IrmEnabled.HasValue)
                list.IrmEnabled = definition.IrmEnabled.Value;

            if (definition.IrmExpire.HasValue)
                list.IrmExpire = definition.IrmExpire.Value;

            if (definition.IrmReject.HasValue)
                list.IrmReject = definition.IrmReject.Value;

            // the rest
            if (definition.EnableAttachments.HasValue)
                list.EnableAttachments = definition.EnableAttachments.Value;

            if (definition.EnableFolderCreation.HasValue)
                list.EnableFolderCreation = definition.EnableFolderCreation.Value;

            if (definition.EnableMinorVersions.HasValue)
                list.EnableMinorVersions = definition.EnableMinorVersions.Value;

            if (definition.EnableModeration.HasValue)
                list.EnableModeration = definition.EnableModeration.Value;

            if (definition.EnableVersioning.HasValue)
                list.EnableVersioning = definition.EnableVersioning.Value;

            if (definition.ForceCheckout.HasValue)
                list.ForceCheckout = definition.ForceCheckout.Value;

            if (definition.Hidden.HasValue)
                list.Hidden = definition.Hidden.Value;

            if (definition.NoCrawl.HasValue)
                list.NoCrawl = definition.NoCrawl.Value;

            if (definition.OnQuickLaunch.HasValue)
                list.OnQuickLaunch = definition.OnQuickLaunch.Value;

            if (definition.MajorVersionLimit.HasValue)
                list.MajorVersionLimit = definition.MajorVersionLimit.Value;

            if (definition.MajorWithMinorVersionsLimit.HasValue)
                list.MajorWithMinorVersionsLimit = definition.MajorWithMinorVersionsLimit.Value;
        }

        private SPList GetOrCreateList(
            object modelHost,
            SPWeb web, ListDefinition listModel)
        {
            var result = GetListByUrl(web, listModel);

            if (result == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list");

                var listId = default(Guid);

                // "SPBug", there are two ways to create lists 
                // (1) by TemplateName (2) by TemplateType 
                if (listModel.TemplateType > 0)
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateType: [{0}]", listModel.TemplateType);

                    //listId = web.Lists.Add(listModel.Url, listModel.Description ?? string.Empty, (SPListTemplateType)listModel.TemplateType);
                    listId = web.Lists.Add(
                                    listModel.Title,
                                    listModel.Description ?? string.Empty,
                                    listModel.GetListUrl(),
                                    string.Empty,
                                    (int)listModel.TemplateType,
                                    string.Empty);
                }
                else if (!string.IsNullOrEmpty(listModel.TemplateName))
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateName: [{0}]", listModel.TemplateName);

                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Searching list template in web.ListTemplates");
                    var listTemplate = ResolveListTemplate(web, listModel);

                    listId = web.Lists.Add(
                                   listModel.Title,
                                   listModel.Description ?? string.Empty,
                                   listModel.GetListUrl(),
                                   listTemplate.FeatureId.ToString(),
                                   (int)listTemplate.Type,
                                   listTemplate.DocumentTemplate);
                }
                else
                {
                    throw new ArgumentException("TemplateType or TemplateName must be defined");
                }

                result = web.Lists[listId];

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = result,
                    ObjectType = typeof(SPList),
                    ObjectDefinition = listModel,
                    ModelHost = modelHost
                });
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = result,
                    ObjectType = typeof(SPList),
                    ObjectDefinition = listModel,
                    ModelHost = modelHost
                });
            }

            return result;
        }

        protected virtual SPListTemplate ResolveListTemplate(SPWeb web, ListDefinition listModel)
        {
            // internal names would be with '.STP', so just a little bit easier to define and find
            var templateName = listModel.TemplateName.ToUpper().Replace(".STP", string.Empty);

            var listTemplate = web.ListTemplates
                .OfType<SPListTemplate>()
                .FirstOrDefault(t => t.InternalName.ToUpper().Replace(".STP", string.Empty) == templateName);

            if (listTemplate == null)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                    "Searching list template in Site.GetCustomListTemplates(web)");

                var customListTemplates = web.Site.GetCustomListTemplates(web);
                listTemplate = customListTemplates
                    .OfType<SPListTemplate>()
                    .FirstOrDefault(t => t.InternalName.ToUpper().Replace(".STP", string.Empty) == templateName);
            }

            if (listTemplate == null)
            {
                throw new SPMeta2Exception(string.Format("Can't find custom list template with internal Name:[{0}]",
                    listModel.TemplateName));
            }
            return listTemplate;
        }

        private static SPList GetListByUrl(SPWeb web, ListDefinition listModel)
        {
            SPList result;

            try
            {
                var targetListUrl = SPUrlUtility.CombineUrl(web.Url, listModel.GetListUrl());
                result = web.GetList(targetListUrl);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        #endregion
    }
}
