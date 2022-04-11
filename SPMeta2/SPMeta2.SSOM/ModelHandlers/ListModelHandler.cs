using System;
using System.Linq;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.ModelHandlers.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ListModelHandler : SingleHostTypedSSOMModelHandlerBase<WebModelHost, ListDefinition, SPList>
    {
        protected override SPList GetCurrentObject(WebModelHost typedModelHost, ListDefinition definition)
        {
            SPList result;

            try
            {
                var web = typedModelHost.HostWeb;

#pragma warning disable 618
                var targetListUrl = SPUrlUtility.CombineUrl(web.Url, definition.GetListUrl());
#pragma warning restore 618
                result = web.GetList(targetListUrl);
            }
            catch
            {
                result = null;
            }

            return result;
        }

        protected override void UpdateObject(SPList currentObject)
        {
            currentObject.Update();
        }

        protected override SPList CreateObject(WebModelHost typedModelHost, ListDefinition definition)
        {
            var listModel = definition;
            var web = typedModelHost.HostWeb;

            TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list");

            Guid listId;

            // create with the random title to avoid issue with 2 lists + diff URL and same Title
            // list Title will be renamed later on
            var listTitle = Guid.NewGuid().ToString("N");

            // "SPBug", there are two ways to create lists 
            // (1) by TemplateName (2) by TemplateType 
            if (listModel.TemplateType > 0)
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateType: [{0}]", listModel.TemplateType);

                //listId = web.Lists.Add(listModel.Url, listModel.Description ?? string.Empty, (SPListTemplateType)listModel.TemplateType);
                listId = web.Lists.Add(
                                listTitle,
                                listModel.Description ?? string.Empty,
#pragma warning disable 618
 listModel.GetListUrl(),
#pragma warning restore 618
 string.Empty,
                                listModel.TemplateType,
                                string.Empty);
            }
            else if (!string.IsNullOrEmpty(listModel.TemplateName))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateName: [{0}]", listModel.TemplateName);

                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Searching list template in web.ListTemplates");
                var listTemplate = ResolveListTemplate(web, listModel);

                listId = web.Lists.Add(
                               listTitle,
                               listModel.Description ?? string.Empty,
#pragma warning disable 618
 listModel.GetListUrl(),
#pragma warning restore 618
 listTemplate.FeatureId.ToString(),
                               (int)listTemplate.Type,
                               listTemplate.DocumentTemplate);
            }
            else
            {
                throw new ArgumentException("TemplateType or TemplateName must be defined");
            }

            return web.Lists[listId];
        }

        protected override void MapObject(SPList currentObject, ListDefinition definition)
        {
            var list = currentObject;

            // temporarily switch culture to allow setting of the properties Title and Description for multi-language scenarios
            CultureUtils.WithCulture(currentObject.ParentWeb.UICulture, () =>
            {
                list.Title = definition.Title;

                // SPBug, again & again, must not be null
                list.Description = definition.Description ?? string.Empty;
            });

            list.ContentTypesEnabled = definition.ContentTypesEnabled;

            if (!string.IsNullOrEmpty(definition.DraftVersionVisibility))
            {
                var draftOption =
                    (DraftVisibilityType)Enum.Parse(typeof(DraftVisibilityType), definition.DraftVersionVisibility);
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

            if (definition.NavigateForFormsPages.HasValue)
                list.NavigateForFormsPages = definition.NavigateForFormsPages.Value;

            if (definition.EnableAssignToEmail.HasValue)
                list.EnableAssignToEmail = definition.EnableAssignToEmail.Value;

            if (definition.DisableGridEditing.HasValue)
                list.DisableGridEditing = definition.DisableGridEditing.Value;

#if !NET35
            if (definition.IndexedRootFolderPropertyKeys.Any())
            {
                foreach (var indexedProperty in definition.IndexedRootFolderPropertyKeys)
                {
                    // indexed prop should exist in the prop bag
                    // otherwise it won't be saved by SharePoint (ILSpy / Refletor to see the logic)
                    // http://rwcchen.blogspot.com.au/2014/06/sharepoint-2013-indexed-property-keys.html

                    var propName = indexedProperty.Name;
                    var propValue = string.IsNullOrEmpty(indexedProperty.Value)
                                            ? string.Empty
                                            : indexedProperty.Value;

                    if (list.RootFolder.Properties.ContainsKey(propName))
                        list.RootFolder.Properties[propName] = propValue;
                    else
                        list.RootFolder.Properties.Add(propName, propValue);

                    if (!list.IndexedRootFolderPropertyKeys.Contains(propName))
                        list.IndexedRootFolderPropertyKeys.Add(propName);
                }
            }
#endif

            if (definition.WriteSecurity.HasValue)
                list.WriteSecurity = definition.WriteSecurity.Value;

            if (definition.ReadSecurity.HasValue)
                list.ReadSecurity = definition.ReadSecurity.Value;

            var docLibrary = list as SPDocumentLibrary;

            if (docLibrary != null)
            {
                if (!string.IsNullOrEmpty(definition.DocumentTemplateUrl))
                {
                    var urlValue = definition.DocumentTemplateUrl;

                    urlValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = urlValue,
                        Context = list.ParentWeb
                    }).Value;

                    if (!urlValue.StartsWith("/")
                        && !urlValue.StartsWith("http:")
                        && !urlValue.StartsWith("https:"))
                    {
                        urlValue = "/" + urlValue;
                    }

                    docLibrary.DocumentTemplateUrl = urlValue;
                }
            }

            ProcessLocalization(list, definition);
        }

        #region utils

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

        #endregion

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            // could either be web or list model host
            // https://github.com/SubPointSolutions/spmeta2/issues/829

            SPWeb web = null;
            SPList hostList = null;

            if (modelHost is WebModelHost)
                web = (modelHost as WebModelHost).HostWeb;
            else if (modelHost is ListModelHost)
            {
                web = (modelHost as ListModelHost).HostList.ParentWeb;
                hostList = (modelHost as ListModelHost).HostList;
            }
            else
            {
                throw new SPMeta2UnsupportedModelHostException(
                    string.Format("Unsupported model host type:[{0}]", modelHost.GetType()));
            }

            var listDefinition = model as ListDefinition;

            if ((web != null || hostList != null) && listDefinition != null)
            {
                if (hostList == null)
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
                        var tmpListId = web.Lists.Add(Guid.NewGuid().ToString(), string.Empty, SPListTemplateType.GenericList);
                        var tmpList = web.Lists[tmpListId];

                        tmpList.Delete();
                    }
                    catch (Exception)
                    {
                    }
                }


#pragma warning disable 618
                var list = hostList ?? web.GetList(SPUtility.ConcatUrls(web.ServerRelativeUrl, listDefinition.GetListUrl()));
#pragma warning restore 618

                var listModelHost = ModelHostBase.Inherit<ListModelHost>(modelHost as ModelHostBase, host =>
                {
                    host.HostList = list;
                });

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

        protected virtual void ProcessLocalization(SPList obj, ListDefinition definition)
        {
            if (definition.TitleResource.Any())
            {
                foreach (var locValue in definition.TitleResource)
                    LocalizationService.ProcessUserResource(obj, obj.TitleResource, locValue);
            }

            if (definition.DescriptionResource.Any())
            {
                foreach (var locValue in definition.DescriptionResource)
                    LocalizationService.ProcessUserResource(obj, obj.DescriptionResource, locValue);
            }
        }

    }

    //public class ListModelHandler : SSOMModelHandlerBase
    //{
    //    #region methods

    //    public override Type TargetType
    //    {
    //        get { return typeof(ListDefinition); }
    //    }

    //    public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
    //    {
    //        var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
    //        var web = webModelHost.HostWeb;

    //        var listDefinition = model as ListDefinition;

    //        if (web != null && listDefinition != null)
    //        {
    //            // This is very important line ->  adding new 'fake list'
    //            //
    //            // Nintex workflow deployment web service updates the list, so that version of the list becomes +4
    //            // Current SPWeb has not been updated, current list will be 4 versions behind so you will have 'Save conflict' exception
    //            //
    //            // We try to add new list, so SPListCollection is invalidated.
    //            // Surely, we won't save this list.
    //            try
    //            {
    //                var tmpListId = web.Lists.Add(Guid.NewGuid().ToString(), string.Empty, Microsoft.SharePoint.SPListTemplateType.GenericList);
    //                var tmpList = web.Lists[tmpListId];
    //                tmpList.Delete();
    //            }
    //            catch (Exception)
    //            {
    //            }

    //            var list = web.GetList(SPUtility.ConcatUrls(web.ServerRelativeUrl, listDefinition.GetListUrl()));

    //            var listModelHost = new ListModelHost
    //            {
    //                HostList = list
    //            };

    //            if (childModelType == typeof(ModuleFileDefinition))
    //            {
    //                var folderModelHost = new FolderModelHost
    //                {
    //                    CurrentLibrary = list as SPDocumentLibrary,
    //                    CurrentLibraryFolder = list.RootFolder,

    //                    CurrentList = (list as SPDocumentLibrary != null) ? null : list,
    //                    CurrentListItem = null,
    //                };

    //                action(folderModelHost);
    //            }
    //            else if (childModelType == typeof(FolderDefinition))
    //            {
    //                var folderModelHost = new FolderModelHost
    //                {
    //                    CurrentLibrary = list as SPDocumentLibrary,
    //                    CurrentLibraryFolder = list.RootFolder,

    //                    CurrentList = (list as SPDocumentLibrary != null) ? null : list,
    //                    CurrentListItem = null,
    //                };

    //                action(folderModelHost);
    //            }
    //            else if (typeof(PageDefinitionBase).IsAssignableFrom(childModelType))
    //            {
    //                var folderModelHost = new FolderModelHost
    //                {
    //                    CurrentLibrary = list as SPDocumentLibrary,
    //                    CurrentLibraryFolder = list.RootFolder,

    //                    CurrentList = (list as SPDocumentLibrary != null) ? null : list,
    //                    CurrentListItem = null,
    //                };

    //                action(folderModelHost);
    //            }

    //            else
    //            {
    //                action(listModelHost);
    //            }

    //            if (listModelHost.ShouldUpdateHost)
    //                list.Update();
    //        }
    //        else
    //        {
    //            action(modelHost);
    //        }
    //    }

    //    public override void DeployModel(object modelHost, DefinitionBase model)
    //    {
    //        var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
    //        var web = webModelHost.HostWeb;

    //        var listModel = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());

    //        // min provision
    //        var targetList = GetOrCreateList(modelHost, web, listModel);

    //        MapListProperties(targetList, listModel);

    //        InvokeOnModelEvent(this, new ModelEventArgs
    //        {
    //            CurrentModelNode = null,
    //            Model = null,
    //            EventType = ModelEventType.OnProvisioned,
    //            Object = targetList,
    //            ObjectType = typeof(SPList),
    //            ObjectDefinition = listModel,
    //            ModelHost = modelHost
    //        });

    //        targetList.Update();
    //    }

    //    private static void MapListProperties(SPList list, ListDefinition definition)
    //    {
    //        list.Title = definition.Title;

    //        // SPBug, again & again, must not be null
    //        list.Description = definition.Description ?? string.Empty;
    //        list.ContentTypesEnabled = definition.ContentTypesEnabled;

    //        if (!string.IsNullOrEmpty(definition.DraftVersionVisibility))
    //        {
    //            var draftOption = (DraftVisibilityType)Enum.Parse(typeof(DraftVisibilityType), definition.DraftVersionVisibility);
    //            list.DraftVersionVisibility = draftOption;
    //        }

    //        // IRM
    //        if (definition.IrmEnabled.HasValue)
    //            list.IrmEnabled = definition.IrmEnabled.Value;

    //        if (definition.IrmExpire.HasValue)
    //            list.IrmExpire = definition.IrmExpire.Value;

    //        if (definition.IrmReject.HasValue)
    //            list.IrmReject = definition.IrmReject.Value;

    //        // the rest
    //        if (definition.EnableAttachments.HasValue)
    //            list.EnableAttachments = definition.EnableAttachments.Value;

    //        if (definition.EnableFolderCreation.HasValue)
    //            list.EnableFolderCreation = definition.EnableFolderCreation.Value;

    //        if (definition.EnableMinorVersions.HasValue)
    //            list.EnableMinorVersions = definition.EnableMinorVersions.Value;

    //        if (definition.EnableModeration.HasValue)
    //            list.EnableModeration = definition.EnableModeration.Value;

    //        if (definition.EnableVersioning.HasValue)
    //            list.EnableVersioning = definition.EnableVersioning.Value;

    //        if (definition.ForceCheckout.HasValue)
    //            list.ForceCheckout = definition.ForceCheckout.Value;

    //        if (definition.Hidden.HasValue)
    //            list.Hidden = definition.Hidden.Value;

    //        if (definition.NoCrawl.HasValue)
    //            list.NoCrawl = definition.NoCrawl.Value;

    //        if (definition.OnQuickLaunch.HasValue)
    //            list.OnQuickLaunch = definition.OnQuickLaunch.Value;

    //        if (definition.MajorVersionLimit.HasValue)
    //            list.MajorVersionLimit = definition.MajorVersionLimit.Value;

    //        if (definition.MajorWithMinorVersionsLimit.HasValue)
    //            list.MajorWithMinorVersionsLimit = definition.MajorWithMinorVersionsLimit.Value;
    //    }

    //    private SPList GetOrCreateList(
    //        object modelHost,
    //        SPWeb web, ListDefinition listModel)
    //    {
    //        var result = GetListByUrl(web, listModel);

    //        if (result == null)
    //        {
    //            TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new list");

    //            var listId = default(Guid);

    //            // "SPBug", there are two ways to create lists 
    //            // (1) by TemplateName (2) by TemplateType 
    //            if (listModel.TemplateType > 0)
    //            {
    //                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateType: [{0}]", listModel.TemplateType);

    //                //listId = web.Lists.Add(listModel.Url, listModel.Description ?? string.Empty, (SPListTemplateType)listModel.TemplateType);
    //                listId = web.Lists.Add(
    //                                listModel.Title,
    //                                listModel.Description ?? string.Empty,
    //                                listModel.GetListUrl(),
    //                                string.Empty,
    //                                (int)listModel.TemplateType,
    //                                string.Empty);
    //            }
    //            else if (!string.IsNullOrEmpty(listModel.TemplateName))
    //            {
    //                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Creating list by TemplateName: [{0}]", listModel.TemplateName);

    //                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Searching list template in web.ListTemplates");
    //                var listTemplate = ResolveListTemplate(web, listModel);

    //                listId = web.Lists.Add(
    //                               listModel.Title,
    //                               listModel.Description ?? string.Empty,
    //                               listModel.GetListUrl(),
    //                               listTemplate.FeatureId.ToString(),
    //                               (int)listTemplate.Type,
    //                               listTemplate.DocumentTemplate);
    //            }
    //            else
    //            {
    //                throw new ArgumentException("TemplateType or TemplateName must be defined");
    //            }

    //            result = web.Lists[listId];

    //            InvokeOnModelEvent(this, new ModelEventArgs
    //            {
    //                CurrentModelNode = null,
    //                Model = null,
    //                EventType = ModelEventType.OnProvisioning,
    //                Object = result,
    //                ObjectType = typeof(SPList),
    //                ObjectDefinition = listModel,
    //                ModelHost = modelHost
    //            });
    //        }
    //        else
    //        {
    //            TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing list");

    //            InvokeOnModelEvent(this, new ModelEventArgs
    //            {
    //                CurrentModelNode = null,
    //                Model = null,
    //                EventType = ModelEventType.OnProvisioning,
    //                Object = result,
    //                ObjectType = typeof(SPList),
    //                ObjectDefinition = listModel,
    //                ModelHost = modelHost
    //            });
    //        }

    //        return result;
    //    }

    //    protected virtual SPListTemplate ResolveListTemplate(SPWeb web, ListDefinition listModel)
    //    {
    //        // internal names would be with '.STP', so just a little bit easier to define and find
    //        var templateName = listModel.TemplateName.ToUpper().Replace(".STP", string.Empty);

    //        var listTemplate = web.ListTemplates
    //            .OfType<SPListTemplate>()
    //            .FirstOrDefault(t => t.InternalName.ToUpper().Replace(".STP", string.Empty) == templateName);

    //        if (listTemplate == null)
    //        {
    //            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
    //                "Searching list template in Site.GetCustomListTemplates(web)");

    //            var customListTemplates = web.Site.GetCustomListTemplates(web);
    //            listTemplate = customListTemplates
    //                .OfType<SPListTemplate>()
    //                .FirstOrDefault(t => t.InternalName.ToUpper().Replace(".STP", string.Empty) == templateName);
    //        }

    //        if (listTemplate == null)
    //        {
    //            throw new SPMeta2Exception(string.Format("Can't find custom list template with internal Name:[{0}]",
    //                listModel.TemplateName));
    //        }
    //        return listTemplate;
    //    }

    //    private static SPList GetListByUrl(SPWeb web, ListDefinition listModel)
    //    {
    //        SPList result;

    //        try
    //        {
    //            var targetListUrl = SPUrlUtility.CombineUrl(web.Url, listModel.GetListUrl());
    //            result = web.GetList(targetListUrl);
    //        }
    //        catch
    //        {
    //            result = null;
    //        }

    //        return result;
    //    }

    //    #endregion
    //}
}
