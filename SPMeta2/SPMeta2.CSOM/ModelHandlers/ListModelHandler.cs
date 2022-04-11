using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.SharePoint.Client;

using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;
using UrlUtility = SPMeta2.Utils.UrlUtility;

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
            var modelHost = modelHostContext.ModelHost as ModelHostBase;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            Web web = null;
            List hostList = null;

            if (modelHost is ListModelHost)
            {
                web = (modelHost as ListModelHost).HostList.ParentWeb;
                hostList = (modelHost as ListModelHost).HostList;
            }
            else if (modelHost is WebModelHost)
            {
                web = (modelHost as WebModelHost).HostWeb;
            }
            else
            {
                throw new SPMeta2UnsupportedModelHostException(
                    string.Format("Unsupported model host type:[{0}]", modelHost.GetType()));
            }

            var listDefinition = model as ListDefinition;
            var context = web.Context;

            if (!web.IsPropertyAvailable("ServerRelativeUrl"))
            {
                context.Load(web, w => w.ServerRelativeUrl);
                context.ExecuteQueryWithTrace();
            }

            if (listDefinition != null && (web != null || hostList != null))
            {
                var list = hostList ?? LoadCurrentList(web, listDefinition);

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

                var listModelHost = ModelHostBase.Inherit<ListModelHost>(modelHost, c =>
                {
                    c.HostList = list;
                });

                if (childModelType == typeof(ListViewDefinition))
                {
                    context.Load(list, l => l.Views);
                    context.ExecuteQueryWithTrace();

                    action(listModelHost);
                }
                else if (childModelType == typeof(ModuleFileDefinition))
                {
                    context.Load(list, l => l.RootFolder);
                    context.Load(list, l => l.BaseType);

                    context.ExecuteQueryWithTrace();

                    var folderModelHost = ModelHostBase.Inherit<FolderModelHost>(modelHost, itemHost =>
                    {
                        itemHost.CurrentWeb = web;
                        itemHost.CurrentList = list;
                    });

                    if (list.BaseType == BaseType.DocumentLibrary)
                    {
                        folderModelHost.CurrentListFolder = list.RootFolder;
                    }
                    else
                    {
                        folderModelHost.CurrentListFolder = list.RootFolder;
                        folderModelHost.CurrentListItem = null;
                    }

                    action(folderModelHost);
                }
                else if (childModelType == typeof(FolderDefinition))
                {
                    context.Load(list, l => l.RootFolder);
                    context.Load(list, l => l.BaseType);

                    context.ExecuteQueryWithTrace();

                    var folderModelHost = ModelHostBase.Inherit<FolderModelHost>(modelHost, itemHost =>
                    {
                        itemHost.CurrentWeb = web;
                        itemHost.CurrentList = list;
                    });

                    if (list.BaseType == BaseType.DocumentLibrary)
                    {
                        folderModelHost.CurrentListFolder = list.RootFolder;
                    }
                    else
                    {
                        folderModelHost.CurrentListFolder = list.RootFolder;
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
                    context.Load(list, l => l.RootFolder);
                    context.Load(list, l => l.BaseType);

                    context.ExecuteQueryWithTrace();

                    var folderModelHost = ModelHostBase.Inherit<FolderModelHost>(modelHost, itemHost =>
                    {
                        itemHost.CurrentWeb = web;
                        itemHost.CurrentList = list;
                    });

                    folderModelHost.CurrentListFolder = list.RootFolder;

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

#pragma warning disable 618
            var listUrl = UrlUtility.CombineUrl(web.ServerRelativeUrl, listModel.GetListUrl());
#pragma warning restore 618

            Folder folder;

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
#if !NET35
                folder = web.GetFolderByServerRelativeUrl(listUrl);

                context.Load(folder.Properties);
                context.ExecuteQueryWithTrace();

                var listId = new Guid(folder.Properties["vti_listname"].ToString());
                var list = web.Lists.GetById(listId);

                context.Load(list);

                if (listModel.IndexedRootFolderPropertyKeys.Any())
                {
                    context.Load(list, l => l.RootFolder.Properties);
                }

                context.ExecuteQueryWithTrace();

                currentList = list;

#endif

#if NET35

                // SP2010 CSOM hack
                // http://impl.com/questions/4284722/sharepoint-2010-client-object-model-get-a-list-item-from-a-url

                var listQuery = from list in web.Lists
                                where list.RootFolder.ServerRelativeUrl == listUrl
                                select list;

                var queryResult = context.LoadQuery(listQuery);
                context.ExecuteQueryWithTrace();

                var resultList = queryResult.FirstOrDefault();

                currentList = resultList;
#endif
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
#pragma warning disable 618
                    Url = listModel.GetListUrl()
#pragma warning restore 618
                };

                if (listModel.TemplateType > 0)
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateType: [{0}]", listModel.TemplateType);

                    listInfo.TemplateType = listModel.TemplateType;
                }
                else if (!string.IsNullOrEmpty(listModel.TemplateName))
                {
                    TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateName: [{0}]", listModel.TemplateName);

                    var listTemplate = ResolveListTemplate(webModelHost, listModel);

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

                currentList.Update();
                context.ExecuteQueryWithTrace();

                currentList = LoadCurrentList(web, listModel);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list");
            }

            MapListProperties(modelHost, currentList, listModel);

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

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Calling currentList.Update()");

            currentList.Update();
            context.ExecuteQueryWithTrace();
        }

        protected virtual ListTemplate ResolveListTemplate(WebModelHost host, ListDefinition listModel)
        {
            var context = host.HostClientContext;

            var site = host.HostSite;
            var web = host.HostWeb;

            // internal names would be with '.STP', so just a little bit easier to define and find
            var templateName = listModel.TemplateName.ToUpper().Replace(".STP", string.Empty);

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Fetching all web.ListTemplates");

            context.Load(web, tmpWeb => tmpWeb.ListTemplates);
            context.ExecuteQueryWithTrace();

            var listTemplate = web.ListTemplates
                                  .FirstOrDefault(t => t.InternalName.ToUpper().Replace(".STP", string.Empty) == templateName);

            if (listTemplate == null)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                    "Searching list template in Site.GetCustomListTemplates(web)");

                var customListTemplates = site.GetCustomListTemplates(web);
                context.Load(customListTemplates);
                context.ExecuteQueryWithTrace();

                listTemplate = customListTemplates
                                  .FirstOrDefault(t => t.InternalName.ToUpper().Replace(".STP", string.Empty) == templateName);
            }

            if (listTemplate == null)
            {
                throw new SPMeta2Exception(string.Format("Can't find custom list template with internal Name:[{0}]",
                    listModel.TemplateName));
            }
            return listTemplate;
        }

        private void MapListProperties(object modelHost, List list, ListDefinition definition)
        {
            var csomModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            var context = list.Context;

            list.Title = definition.Title;
            list.Description = definition.Description ?? string.Empty;
            list.ContentTypesEnabled = definition.ContentTypesEnabled;

            if (definition.Hidden.HasValue)
                list.Hidden = definition.Hidden.Value;

            if (!string.IsNullOrEmpty(definition.DraftVersionVisibility))
            {
                var draftOption = (DraftVisibilityType)Enum.Parse(typeof(DraftVisibilityType), definition.DraftVersionVisibility);
                list.DraftVersionVisibility = draftOption;
            }

#if !NET35
            // IRM
            if (definition.IrmEnabled.HasValue)
                list.IrmEnabled = definition.IrmEnabled.Value;

            if (definition.IrmExpire.HasValue)
                list.IrmExpire = definition.IrmExpire.Value;

            if (definition.IrmReject.HasValue)
                list.IrmReject = definition.IrmReject.Value;

#endif

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
            {
                if (ReflectionUtils.HasProperty(list, "MajorVersionLimit"))
                {
                    ClientRuntimeQueryService.SetProperty(list, "MajorVersionLimit", definition.MajorVersionLimit.Value);
                }
                else
                {
                    TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                        string.Format(
                            "CSOM runtime doesn't have [{0}] methods support. Update CSOM runtime to a new version. Provision is skipped",
                            string.Join(", ", new string[] { "MajorVersionLimit" })));
                }
            }

            if (definition.MajorWithMinorVersionsLimit.HasValue)
            {
                if (ReflectionUtils.HasProperty(list, "MajorWithMinorVersionsLimit"))
                {
                    ClientRuntimeQueryService.SetProperty(list, "MajorWithMinorVersionsLimit", definition.MajorWithMinorVersionsLimit.Value);
                }
                else
                {
                    TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                        string.Format(
                            "CSOM runtime doesn't have [{0}] methods support. Update CSOM runtime to a new version. Provision is skipped",
                            string.Join(", ", new string[] { "MajorWithMinorVersionsLimit" })));
                }
            }

            if (definition.ReadSecurity.HasValue)
            {
                if (ReflectionUtils.HasProperty(list, "ReadSecurity"))
                    ClientRuntimeQueryService.SetProperty(list, "ReadSecurity", definition.ReadSecurity.Value);
                else
                {
                    TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                        "CSOM runtime doesn't have List.ReadSecurity. Update CSOM runtime to a new version. Provision is skipped");
                }
            }

            if (definition.WriteSecurity.HasValue)
            {
                if (ReflectionUtils.HasProperty(list, "WriteSecurity"))
                    ClientRuntimeQueryService.SetProperty(list, "WriteSecurity", definition.WriteSecurity.Value);
                else
                {
                    TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                        "CSOM runtime doesn't have List.WriteSecurity. Update CSOM runtime to a new version. Provision is skipped");
                }
            }

            if (!string.IsNullOrEmpty(definition.DocumentTemplateUrl))
            {
                var urlValue = definition.DocumentTemplateUrl;

                urlValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = urlValue,
                    Context = csomModelHost
                }).Value;

                if (!urlValue.StartsWith("/")
                    && !urlValue.StartsWith("http:")
                    && !urlValue.StartsWith("https:"))
                {
                    urlValue = "/" + urlValue;
                }

                list.DocumentTemplateUrl = urlValue;
            }

            ProcessLocalization(list, definition);

#if !NET35
            if (definition.IndexedRootFolderPropertyKeys.Any())
            {
                var props = list.RootFolder.Properties;

                // may not be there at all
                var indexedPropertyValue = props.FieldValues.Keys.Contains("vti_indexedpropertykeys")
                                            ? ConvertUtils.ToStringAndTrim(props["vti_indexedpropertykeys"])
                                            : string.Empty;

                var currentIndexedProperties = IndexedPropertyUtils.GetDecodeValueForSearchIndexProperty(indexedPropertyValue);

                // setup property bag
                foreach (var indexedProperty in definition.IndexedRootFolderPropertyKeys)
                {
                    // indexed prop should exist in the prop bag
                    // otherwise it won't be saved by SharePoint (ILSpy / Refletor to see the logic)
                    // http://rwcchen.blogspot.com.au/2014/06/sharepoint-2013-indexed-property-keys.html

                    var propName = indexedProperty.Name;
                    var propValue = string.IsNullOrEmpty(indexedProperty.Value)
                                            ? string.Empty
                                            : indexedProperty.Value;

                    props[propName] = propValue;
                }

                // merge and setup indexed prop keys, preserve existing props
                foreach (var indexedProperty in definition.IndexedRootFolderPropertyKeys)
                {
                    if (!currentIndexedProperties.Contains(indexedProperty.Name))
                        currentIndexedProperties.Add(indexedProperty.Name);
                }

                props["vti_indexedpropertykeys"] = IndexedPropertyUtils.GetEncodedValueForSearchIndexProperty(currentIndexedProperties);
                list.RootFolder.Update();
            }
#endif
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

        #endregion

        public override Type TargetType
        {
            get { return typeof(ListDefinition); }
        }

        protected virtual void ProcessLocalization(List obj, ListDefinition definition)
        {
            ProcessGenericLocalization(obj, new Dictionary<string, List<ValueForUICulture>>
            {
                { "TitleResource", definition.TitleResource },
                { "DescriptionResource", definition.DescriptionResource },
            });
        }
    }
}
