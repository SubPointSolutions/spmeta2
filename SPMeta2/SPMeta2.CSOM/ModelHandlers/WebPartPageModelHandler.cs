using System;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.ModelHosts;

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

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var folderModelHost = modelHost as FolderModelHost;
            var webPartPageDefinition = model as WebPartPageDefinition;

            Folder folder = folderModelHost.CurrentLibraryFolder;

            if (folder != null && webPartPageDefinition != null)
            {
                var context = folder.Context;
                var currentPage = GetCurrentWebPartPage(folderModelHost.CurrentList, folder, GetSafeWebPartPageFileName(webPartPageDefinition));

                var currentListItem = currentPage.ListItemAllFields;
                context.Load(currentListItem);
                context.ExecuteQuery();

                if (typeof(WebPartDefinitionBase).IsAssignableFrom(childModelType))
                {
                    var listItemHost = ModelHostBase.Inherit<ListItemModelHost>(folderModelHost, itemHost =>
                    {
                        itemHost.HostListItem = currentListItem;
                    });

                    action(listItemHost);

                    //currentListItem.Update();
                }

                context.ExecuteQuery();
            }
            else
            {
                action(modelHost);
            }
        }

        protected ListItem SearchItemByName(List list, Folder folder, string pageName)
        {
            var context = list.Context;

            if (folder != null)
            {
                if (!folder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    folder.Context.Load(folder, f => f.ServerRelativeUrl);
                    folder.Context.ExecuteQuery();
                }
            }

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
            context.ExecuteQuery();

            return collListItems.FirstOrDefault();

        }

        protected File GetCurrentWebPartPage(List list, Folder folder, string pageName)
        {
            var context = folder.Context;

            //var files = folder.Files;
            //context.Load(files);
            //context.ExecuteQuery();

            //foreach (var file in files)
            //{
            //    if (file.Name.ToUpper() == pageName.ToUpper())
            //        return file;
            //}

            var item = SearchItemByName(list, folder, pageName);

            return item != null ? item.File : null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost as FolderModelHost;
            var webPartPageModel = model as WebPartPageDefinition;

            Folder folder = folderModelHost.CurrentLibraryFolder;

            //if (!string.IsNullOrEmpty(webPartPageModel.FolderUrl))
            //    throw new NotImplementedException("FolderUrl for the web part page model is not supported yet");

            var context = folder.Context;

            // #SPBug
            // it turns out that there is no support for the web part page creating via CMOM
            // we we need to get a byte array to 'hack' this pages out..
            // http://stackoverflow.com/questions/6199990/creating-a-sharepoint-2010-page-via-the-client-object-model
            // http://social.technet.microsoft.com/forums/en-US/sharepointgeneralprevious/thread/6565bac1-daf0-4215-96b2-c3b64270ec08

            var currentPage = GetCurrentWebPartPage(folderModelHost.CurrentList, folder, GetSafeWebPartPageFileName(webPartPageModel));

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = currentPage,
                ObjectType = typeof(File),
                ObjectDefinition = webPartPageModel,
                ModelHost = modelHost
            });

            if ((currentPage == null) || (currentPage != null && webPartPageModel.NeedOverride))
            {
                var file = new FileCreationInformation();

                var pageContent = string.Empty;

                if (!string.IsNullOrEmpty(webPartPageModel.CustomPageLayout))
                    pageContent = webPartPageModel.CustomPageLayout;
                else
                    pageContent = GetWebPartTemplateContent(webPartPageModel);

                var fileName = GetSafeWebPartPageFileName(webPartPageModel);

                file.Url = fileName;
                file.Content = Encoding.UTF8.GetBytes(pageContent);
                file.Overwrite = webPartPageModel.NeedOverride;

                var newFile = folder.Files.Add(file);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = newFile,
                    ObjectType = typeof(File),
                    ObjectDefinition = webPartPageModel,
                    ModelHost = modelHost
                });

                context.Load(newFile);
                context.ExecuteQuery();
            }
            else
            {

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = currentPage,
                    ObjectType = typeof(File),
                    ObjectDefinition = webPartPageModel,
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

        public static string GetWebPartTemplateContent(WebPartPageDefinition webPartPageModel)
        {
            // gosh! would u like to offer a better way?
            switch (webPartPageModel.PageLayoutTemplate)
            {
                case 1:
                    return WebPartPageTemplates.spstd1;
                case 2:
                    return WebPartPageTemplates.spstd2;
                case 3:
                    return WebPartPageTemplates.spstd3;
                case 4:
                    return WebPartPageTemplates.spstd4;
                case 5:
                    return WebPartPageTemplates.spstd5;
                case 6:
                    return WebPartPageTemplates.spstd6;
                case 7:
                    return WebPartPageTemplates.spstd7;
                case 8:
                    return WebPartPageTemplates.spstd8;
            }

            throw new Exception(string.Format("PageLayoutTemplate: [{0}] is not supported.", webPartPageModel.PageLayoutTemplate));
        }

        #endregion
    }
}
