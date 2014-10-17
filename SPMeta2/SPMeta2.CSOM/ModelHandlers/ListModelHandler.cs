using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Utils;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.ModelHosts;
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
            context.ExecuteQuery();

            if (web != null && listDefinition != null)
            {
                // TODO
                // no no no no... not a TITLE! 

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
                    context.ExecuteQuery();

                    action(list);
                }
                else if (childModelType == typeof(ModuleFileDefinition))
                {
                    context.Load<List>(list, l => l.RootFolder);
                    context.Load<List>(list, l => l.BaseType);

                    context.ExecuteQuery();

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

                    context.ExecuteQuery();

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
                else if (childModelType == typeof(SP2013WorkflowSubscriptionDefinition))
                {
                    var sp2013WorkflowSubscriptionModelHost =
                        ModelHostBase.Inherit<SP2013WorkflowSubscriptionModelHost>(webModelHost, host =>
                        {
                            host.HostList = list;
                        });

                    action(sp2013WorkflowSubscriptionModelHost);
                }
                else if (typeof(PageDefinitionBase).IsAssignableFrom(childModelType))
                {
                    context.Load<List>(list, l => l.RootFolder);
                    context.Load<List>(list, l => l.BaseType);

                    context.ExecuteQuery();

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

            context.ExecuteQuery();

            if (!scope.HasException && folder != null && folder.ServerObjectIsNull != true)
            {
                folder = web.GetFolderByServerRelativeUrl(listUrl);
                context.Load(folder.Properties);
                context.ExecuteQuery();

                var listId = new Guid(folder.Properties["vti_listname"].ToString());
                var list = web.Lists.GetById(listId);

                context.Load(list);
                context.ExecuteQuery();

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

            // 1
            context.Load(web, w => w.ServerRelativeUrl);
            context.ExecuteQuery();

            //context.Load(web, w => w.Lists);
            //context.Load(web, w => w.ServerRelativeUrl);
            //var lists = context.LoadQuery<List>(web.Lists.Include(l => l.DefaultViewUrl));
            //context.ExecuteQuery();



            List currentList = null;


            var loadedList = LoadCurrentList(web, listModel);

            if (loadedList != null)
                currentList = loadedList;


            //Folder d;

            ////d.ListItemAllFields.pa

            //context.Load(lists, all => all.Where(l => l.RootFolder.ServerRelativeUrl == listUrl).Include(l => l.Id));
            //context.ExecuteQuery();
            //var list = lists.FirstOrDefault();

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
                // no support for the TemplateName yet
                var listInfo = new ListCreationInformation
                {
                    Title = listModel.Title,
                    Description = listModel.Description ?? string.Empty,
                    Url = listModel.GetListUrl()
                };

                if (listModel.TemplateType > 0)
                {
                    listInfo.TemplateType = listModel.TemplateType;
                }
                else if (!string.IsNullOrEmpty(listModel.TemplateName))
                {
                    context.Load(web, tmpWeb => tmpWeb.ListTemplates);
                    context.ExecuteQuery();

                    // gosh..
                    var listTemplate = FindListTemplateByInternalName(web.ListTemplates, listModel.TemplateName);

                    listInfo.TemplateFeatureId = listTemplate.FeatureId;
                    listInfo.TemplateType = listTemplate.ListTemplateTypeKind;
                }
                else
                {
                    throw new ArgumentException("Either TemplateType or TemplateName has to bbe specified.");
                }

                var newList = web.Lists.Add(listInfo);
                currentList = newList;
            }

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

            currentList.Update();

            context.ExecuteQuery();
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
