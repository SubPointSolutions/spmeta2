using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
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

            targetList.Title = listModel.Title;

            // SPBug, again & again, must not be null
            targetList.Description = listModel.Description ?? string.Empty;
            targetList.ContentTypesEnabled = listModel.ContentTypesEnabled;

            if (listModel.IrmEnabled.HasValue)
                targetList.IrmEnabled = listModel.IrmEnabled.Value;

            if (listModel.IrmExpire.HasValue)
                targetList.IrmExpire = listModel.IrmExpire.Value;

            if (listModel.IrmReject.HasValue)
                targetList.IrmReject = listModel.IrmReject.Value;

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
                                    listModel.Url,
                                    listModel.Description ?? string.Empty,
                                    listModel.GetListUrl(),
                                    string.Empty,
                                    (int)listModel.TemplateType,
                                    string.Empty);
                }
                else if (!string.IsNullOrEmpty(listModel.TemplateName))
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateName: [{0}]", listModel.TemplateName);

                    var listTemplate = web.ListTemplates
                                           .OfType<SPListTemplate>()
                                           .FirstOrDefault(t => t.InternalName == listModel.TemplateName);

                    //listId = web.Lists.Add(listModel.Url, listModel.Description ?? string.Empty, listTemplate);
                    listId = web.Lists.Add(
                                   listModel.Url,
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
