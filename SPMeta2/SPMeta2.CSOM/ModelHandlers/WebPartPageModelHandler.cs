using System;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Utils;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.ModelHosts;
using SPMeta2.Templates;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WebPartPageModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebPartPageDefinition); }
        }

        #endregion

        #region methods

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var folderModelHost = modelHost as FolderModelHost;
            var webPartPageDefinition = model as WebPartPageDefinition;

            Folder folder = folderModelHost.CurrentListFolder;

            if (folder != null && webPartPageDefinition != null)
            {
                var context = folder.Context;
                var currentPage = GetCurrentWebPartPageFile(folderModelHost.CurrentList, folder, GetSafeWebPartPageFileName(webPartPageDefinition));

                //var currentListItem = currentPage.ListItemAllFields;

                //context.Load(currentListItem);
                context.ExecuteQueryWithTrace();

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

                    //currentListItem.Update();
                }
                else if (typeof(BreakRoleInheritanceDefinition).IsAssignableFrom(childModelType)
                        || typeof(SecurityGroupLinkDefinition).IsAssignableFrom(childModelType))
                {

                    var currentListItem = currentPage.ListItemAllFields;

                    context.Load(currentListItem);
                    context.ExecuteQueryWithTrace();

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
            else
            {
                action(modelHost);
            }
        }

        protected File SearchFileByName(List list, Folder folder, string pageName)
        {
            var context = list.Context;

            if (folder != null)
            {
                if (!folder.IsPropertyAvailable("ServerRelativeUrl")
                    // || !folder.IsPropertyAvailable("Properties"))
                    )
                {
                    folder.Context.Load(folder, f => f.ServerRelativeUrl);
                    //folder.Context.Load(folder, f => f.Properties);

                    folder.Context.ExecuteQueryWithTrace();
                }
            }



            // one more time..
            var dQuery = new CamlQuery();

            string QueryString = "<View><Query><Where>" +
                                 "<Eq>" +
                                 "<FieldRef Name=\"FileLeafRef\"/>" +
                                 "<Value Type=\"Text\">" + pageName + "</Value>" +
                                 "</Eq>" +
                                 "</Where></Query></View>";

            dQuery.ViewXml = QueryString;

            if (folder != null)
                dQuery.FolderServerRelativeUrl = folder.ServerRelativeUrl;

            var collListItems = list.GetItems(dQuery);

            context.Load(collListItems);
            context.ExecuteQueryWithTrace();

            var item = collListItems.FirstOrDefault();
            if (item != null)
                return item.File;

            //one more time
            // by full path
            var fileServerRelativePath = UrlUtility.CombineUrl(folder.ServerRelativeUrl, pageName);

            File file = null;

            var scope = new ExceptionHandlingScope(context);
            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    file = list.ParentWeb.GetFileByServerRelativeUrl(fileServerRelativePath);

                    context.Load(file);

                }

                using (scope.StartCatch())
                {

                }
            }

            context.ExecuteQueryWithTrace();

            // Forms folder im the libraries
            // otherwise pure list items search
            if (!scope.HasException && file != null && file.ServerObjectIsNull != null)
            {
                context.Load(file);
                context.Load(file, f => f.Exists);

                context.ExecuteQueryWithTrace();

                if (file.Exists)
                    return file;
            }

            return null;

        }

        protected File GetCurrentWebPartPageFile(List list, Folder folder, string pageName)
        {
            var context = folder.Context;

            //var files = folder.Files;
            //context.Load(files);
            //context.ExecuteQueryWithTrace();

            //foreach (var file in files)
            //{
            //    if (file.Name.ToUpper() == pageName.ToUpper())
            //        return file;
            //}

            return SearchFileByName(list, folder, pageName);
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost as FolderModelHost;
            var definition = model as WebPartPageDefinition;

            var contentTypeId = string.Empty;

            // pre load content type
            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                contentTypeId = definition.ContentTypeId;

            }
            else if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                contentTypeId = ContentTypeLookupService
                                            .LookupContentTypeByName(folderModelHost.CurrentList, definition.ContentTypeName)
                                            .Id.ToString();
            }

            Folder folder = folderModelHost.CurrentListFolder;

            //if (!string.IsNullOrEmpty(webPartPageModel.FolderUrl))
            //    throw new NotImplementedException("FolderUrl for the web part page model is not supported yet");

            var context = folder.Context;

            // #SPBug
            // it turns out that there is no support for the web part page creating via CMOM
            // we we need to get a byte array to 'hack' this pages out..
            // http://stackoverflow.com/questions/6199990/creating-a-sharepoint-2010-page-via-the-client-object-model
            // http://social.technet.microsoft.com/forums/en-US/sharepointgeneralprevious/thread/6565bac1-daf0-4215-96b2-c3b64270ec08

            var currentPage = GetCurrentWebPartPageFile(folderModelHost.CurrentList, folder, GetSafeWebPartPageFileName(definition));

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentPage,
                ObjectType = typeof(File),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if ((currentPage == null) || (currentPage != null && definition.NeedOverride))
            {
                if (definition.NeedOverride)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing web part page");
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "NeedOverride = true. Replacing web part page.");
                }
                else
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new web part page");
                }

                var file = new FileCreationInformation();

                var pageContent = string.Empty;

                if (!string.IsNullOrEmpty(definition.CustomPageLayout))
                    pageContent = definition.CustomPageLayout;
                else
                    pageContent = GetWebPartTemplateContent(definition);

                var fileName = GetSafeWebPartPageFileName(definition);

                file.Url = fileName;
                file.Content = Encoding.UTF8.GetBytes(pageContent);
                file.Overwrite = definition.NeedOverride;

                var newFile = folder.Files.Add(file);

                FieldLookupService.EnsureDefaultValues(newFile.ListItemAllFields, definition.DefaultValues);
                
                if (!string.IsNullOrEmpty(contentTypeId))
                {
                    newFile.ListItemAllFields[BuiltInInternalFieldNames.ContentTypeId] = contentTypeId;
                }

                FieldLookupService.EnsureValues(newFile.ListItemAllFields, definition.Values, true);

                if (definition.Values.Any()
                    || definition.DefaultValues.Any()
                    || !string.IsNullOrEmpty(contentTypeId))
                {
                    newFile.ListItemAllFields.Update();
                    context.ExecuteQueryWithTrace();
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newFile,
                    ObjectType = typeof(File),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                context.Load(newFile);
                context.ExecuteQueryWithTrace();
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing web part page");
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "NeedOverride = false. Skipping replacing web part page.");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentPage,
                    ObjectType = typeof(File),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });
            }

        }

        protected string GetSafeWebPartPageFileName(WebPartPageDefinition webPartPageModel)
        {
            var fileName = webPartPageModel.FileName;
            if (!fileName.EndsWith(".aspx")) fileName += ".aspx";

            return fileName;
        }

        public virtual string GetWebPartTemplateContent(WebPartPageDefinition webPartPageModel)
        {
            // gosh! would u like to offer a better way?
            // always SP2013 for CSOM yet
            // Built-in web part page templates should be correctly resolved for SP2010/2013 #683

            // TODO, sp2016 support, if any
            switch (webPartPageModel.PageLayoutTemplate)
            {
                case 1:
                    return SP2013WebPartPageTemplates.spstd1;
                case 2:
                    return SP2013WebPartPageTemplates.spstd2;
                case 3:
                    return SP2013WebPartPageTemplates.spstd3;
                case 4:
                    return SP2013WebPartPageTemplates.spstd4;
                case 5:
                    return SP2013WebPartPageTemplates.spstd5;
                case 6:
                    return SP2013WebPartPageTemplates.spstd6;
                case 7:
                    return SP2013WebPartPageTemplates.spstd7;
                case 8:
                    return SP2013WebPartPageTemplates.spstd8;
            }

            throw new Exception(string.Format("PageLayoutTemplate: [{0}] is not supported.", webPartPageModel.PageLayoutTemplate));
        }

        #endregion
    }
}
