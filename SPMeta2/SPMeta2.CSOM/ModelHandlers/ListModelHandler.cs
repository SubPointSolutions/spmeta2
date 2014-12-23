using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Utils;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ListModelHandler : CSOMModelHandlerBase
    {
        #region properties

        #endregion

        #region methods

        //public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        //override with

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;
            var listDefinition = model as ListDefinition;
            var context = web.Context;

            context.Load(web, w => w.ServerRelativeUrl);
            context.ExecuteQueryWithTrace();

            if (web != null && listDefinition != null)
            {
                var list = LoadCurrentList(web, listDefinition);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = modelHostContext.ModelNode,
                    Model = null,
                    EventType = ModelEventType.OnModelHostResolving,
                    Object = list,
                    ObjectType = typeof(List),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });

                var listModelHost = ModelHostBase.Inherit<ListModelHost>(webModelHost, c =>
                {
                    c.HostList = list;
                });

                if (childModelType == typeof(ListViewDefinition))
                {
                    context.Load<List>(list, l => l.Views);
                    context.ExecuteQueryWithTrace();

                    action(list);
                }
                else if (childModelType == typeof(ModuleFileDefinition))
                {
                    context.Load<List>(list, l => l.RootFolder);
                    context.Load<List>(list, l => l.BaseType);

                    context.ExecuteQueryWithTrace();

                    var folderModelHost = ModelHostBase.Inherit<FolderModelHost>(webModelHost, itemHost =>
                    {
                        itemHost.CurrentWeb = web;
                        itemHost.CurrentList = list;
                    });

                    if (list.BaseType == BaseType.DocumentLibrary)
                    {
                        folderModelHost.CurrentLibraryFolder = list.RootFolder;
                    }
                    else
                    {
                        folderModelHost.CurrentListItem = null;
                    }

                    action(folderModelHost);
                }
                else if (childModelType == typeof(FolderDefinition))
                {
                    context.Load<List>(list, l => l.RootFolder);
                    context.Load<List>(list, l => l.BaseType);

                    context.ExecuteQueryWithTrace();

                    var folderModelHost = ModelHostBase.Inherit<FolderModelHost>(webModelHost, itemHost =>
                    {
                        itemHost.CurrentWeb = web;
                        itemHost.CurrentList = list;
                    });

                    if (list.BaseType == BaseType.DocumentLibrary)
                    {
                        folderModelHost.CurrentLibraryFolder = list.RootFolder;
                    }
                    else
                    {
                        folderModelHost.CurrentListItem = null;
                    }

                    action(folderModelHost);
                }
                //else if (childModelType == typeof(SP2013WorkflowSubscriptionDefinition))
                //{
                //    var sp2013WorkflowSubscriptionModelHost =
                //        ModelHostBase.Inherit<SP2013WorkflowSubscriptionModelHost>(webModelHost, host =>
                //        {
                //            host.HostList = list;
                //        });

                //    action(sp2013WorkflowSubscriptionModelHost);
                //}
                else if (typeof(PageDefinitionBase).IsAssignableFrom(childModelType))
                {
                    context.Load<List>(list, l => l.RootFolder);
                    context.Load<List>(list, l => l.BaseType);

                    context.ExecuteQueryWithTrace();

                    var folderModelHost = ModelHostBase.Inherit<FolderModelHost>(webModelHost, itemHost =>
                    {
                        itemHost.CurrentWeb = web;
                        itemHost.CurrentList = list;
                    });


                    if (list.BaseType == BaseType.DocumentLibrary)
                    {
                        folderModelHost.CurrentLibraryFolder = list.RootFolder;
                    }
                    else
                    {
                        folderModelHost.CurrentListItem = null;
                    }

                    action(folderModelHost);
                }
                else
                {
                    action(listModelHost);
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = modelHostContext.ModelNode,
                    Model = null,
                    EventType = ModelEventType.OnModelHostResolved,
                    Object = list,
                    ObjectType = typeof(List),
                    ObjectDefinition = model,
                    ModelHost = modelHost
                });

                if (listModelHost.ShouldUpdateHost)
                    list.Update();
            }
            else
            {
                action(modelHost);
            }
        }

        private static List LoadCurrentList(Web web, ListDefinition listModel)
        {
            var context = web.Context;

            List currentList = null;

            var listUrl = UrlUtility.CombineUrl(web.ServerRelativeUrl, listModel.GetListUrl());

            Folder folder = null;

            var scope = new ExceptionHandlingScope(context);

            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    folder = web.GetFolderByServerRelativeUrl(listUrl);
                    context.Load(folder);
                }

                using (scope.StartCatch())
                {

                }
            }

            context.ExecuteQueryWithTrace();

            if (!scope.HasException && folder != null && folder.ServerObjectIsNull != true)
            {
                folder = web.GetFolderByServerRelativeUrl(listUrl);
                context.Load(folder.Properties);
                context.ExecuteQueryWithTrace();

                var listId = new Guid(folder.Properties["vti_listname"].ToString());
                var list = web.Lists.GetById(listId);

                context.Load(list);
                context.ExecuteQueryWithTrace();

                currentList = list;
            }

            return currentList;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());

            var web = webModelHost.HostWeb;
            var listModel = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());

            var context = web.Context;

            context.Load(web, w => w.ServerRelativeUrl);
            context.ExecuteQueryWithTrace();

            List currentList = null;

            var loadedList = LoadCurrentList(web, listModel);

            if (loadedList != null)
                currentList = loadedList;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(List),
                ObjectDefinition = model,
                ModelHost = modelHost
            });
            InvokeOnModelEvent<ListDefinition, List>(currentList, ModelEventType.OnUpdating);

            // gosh!
            //currentList = FindListByUrl(lists, listModel.GetListUrl());

            if (currentList == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list");

                // no support for the TemplateName yet
                var listInfo = new ListCreationInformation
                {
                    Title = listModel.Title,
                    Description = listModel.Description ?? string.Empty,
                    Url = listModel.GetListUrl()
                };

                if (listModel.TemplateType > 0)
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateType: [{0}]", listModel.TemplateType);

                    listInfo.TemplateType = listModel.TemplateType;
                }
                else if (!string.IsNullOrEmpty(listModel.TemplateName))
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateName: [{0}]", listModel.TemplateName);

                    context.Load(web, tmpWeb => tmpWeb.ListTemplates);
                    context.ExecuteQueryWithTrace();

                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Fetching all list templates and matching target one.");
                    var listTemplate = FindListTemplateByInternalName(web.ListTemplates, listModel.TemplateName);

                    listInfo.TemplateFeatureId = listTemplate.FeatureId;
                    listInfo.TemplateType = listTemplate.ListTemplateTypeKind;
                }
                else
                {
                    TraceService.Error((int)LogEventId.ModelProvisionCoreCall, "Either TemplateType or TemplateName has to be specified. Throwing SPMeta2Exception");

                    throw new SPMeta2Exception("Either TemplateType or TemplateName has to be specified.");
                }

                var newList = web.Lists.Add(listInfo);
                currentList = newList;
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list");
            }

            if (listModel.IrmEnabled.HasValue)
                currentList.IrmEnabled = listModel.IrmEnabled.Value;

            if (listModel.IrmExpire.HasValue)
                currentList.IrmExpire = listModel.IrmExpire.Value;

            if (listModel.IrmReject.HasValue)
                currentList.IrmReject = listModel.IrmReject.Value;

            currentList.Title = listModel.Title;
            currentList.Description = listModel.Description ?? string.Empty;
            currentList.ContentTypesEnabled = listModel.ContentTypesEnabled;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentList,
                ObjectType = typeof(List),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            InvokeOnModelEvent<ListDefinition, List>(currentList, ModelEventType.OnUpdated);

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling currentList.Update()");
            currentList.Update();
            context.ExecuteQueryWithTrace();
        }

        public static List FindListByUrl(IEnumerable<List> listCollection, string listUrl)
        {
            foreach (var list in listCollection)
            {
                if (list.DefaultViewUrl.ToUpper().Contains("/" + listUrl.ToUpper() + "/"))
                    return list;
            }

            return null;
        }

        //protected List FindListByTitle(ListCollection listCollection, string listTitle)
        //{
        //    foreach (var list in listCollection)
        //    {
        //        if (System.String.Compare(list.Title, listTitle, System.StringComparison.OrdinalIgnoreCase) == 0)
        //            return list;
        //    }

        //    return null;
        //}

        protected ListTemplate FindListTemplateByInternalName(IEnumerable<ListTemplate> listTemplateCollection, string listTemplateInternalName)
        {
            foreach (var listTemplate in listTemplateCollection)
            {
                if (System.String.Compare(listTemplate.InternalName, listTemplateInternalName, System.StringComparison.OrdinalIgnoreCase) == 0)
                    return listTemplate;
            }

            return null;
        }

        #endregion

        public override Type TargetType
        {
            get { return typeof(ListDefinition); }
        }
    }
}
