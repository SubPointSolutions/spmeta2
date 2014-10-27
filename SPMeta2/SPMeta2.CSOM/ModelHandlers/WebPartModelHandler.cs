using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WebParts;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Common;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using WebPartDefinition = SPMeta2.Definitions.WebPartDefinition;
using System.Xml;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Exceptions;
using System.Text;
using System.IO;
using File = Microsoft.SharePoint.Client.File;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WebPartModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebPartDefinition); }
        }

        // TODO, depends on SP version

        #endregion

        #region methods

        protected void WithWithExistingWebPart(ListItem listItem, WebPartDefinition webPartModel,
             Action<WebPart> action)
        {
            var context = listItem.Context;
            var filePath = listItem["FileRef"].ToString();

            var web = listItem.ParentList.ParentWeb;

            var pageFile = web.GetFileByServerRelativeUrl(filePath);
            var webPartManager = pageFile.GetLimitedWebPartManager(PersonalizationScope.Shared);

            // web part on the page
            var webpartOnPage = webPartManager.WebParts.Include(wp => wp.Id, wp => wp.WebPart);
            var webPartDefenitions = context.LoadQuery(webpartOnPage);

            context.ExecuteQuery();

            var existingWebPart = FindExistingWebPart(webPartDefenitions, webPartModel);

            action(existingWebPart);
        }

        protected File GetCurrentPageFile(ListItemModelHost listItemModelHost)
        {
            var listItem = listItemModelHost.HostListItem;
            var filePath = listItem["FileRef"].ToString();

            var web = listItem.ParentList.ParentWeb;
            return web.GetFileByServerRelativeUrl(filePath);
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

        private static Dictionary<string, string> _wpCache = new Dictionary<string, string>();
        private static object _wpCacheLock = new object();

        protected virtual string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var result = string.Empty;
            var context = listItemModelHost.HostListItem.Context;

            if (!string.IsNullOrEmpty(webPartModel.WebpartFileName))
            {
                lock (_wpCacheLock)
                {
                    var wpKey = webPartModel.WebpartFileName.ToLower();

                    if (_wpCache.ContainsKey(wpKey))
                        result = _wpCache[wpKey];
                    else
                    {

                        var rootWeb = listItemModelHost.HostSite.RootWeb;
                        var rootWebContext = rootWeb.Context;

                        var webPartCatalog =
                            rootWeb.QueryAndGetListByUrl(BuiltInListDefinitions.Calalogs.Wp.GetListUrl());
                        //var webParts = webPartCatalog.GetItems(CamlQuery.CreateAllItemsQuery());

                        //rootWebContext.Load(webParts);
                        //rootWebContext.ExecuteQuery();

                        ListItem targetWebPart = SearchItemByName(webPartCatalog, webPartCatalog.RootFolder,
                            webPartModel.WebpartFileName);

                        //foreach (var webPart in webParts)
                        //    if (webPart["FileLeafRef"].ToString().ToUpper() == webPartModel.WebpartFileName.ToUpper())
                        //        targetWebPart = webPart;

                        if (targetWebPart == null)
                            throw new SPMeta2Exception(string.Format("Cannot find web part file by name:[{0}]",
                                webPartModel.WebpartFileName));

                        var webPartFile = targetWebPart.File;

                        rootWebContext.Load(webPartFile);
                        rootWebContext.ExecuteQuery();


                        // does not work for apps - https://github.com/SubPointSolutions/spmeta2/issues/174
                        //var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(listItemModelHost.HostClientContext, webPartFile.ServerRelativeUrl);

                        //using (var reader = new StreamReader(fileInfo.Stream))
                        //{
                        //    webPartXML = reader.ReadToEnd();
                        //}

                        var data = webPartFile.OpenBinaryStream();

                        context.Load(webPartFile);
                        context.ExecuteQuery();

                        using (var reader = new StreamReader(data.Value))
                        {
                            result = reader.ReadToEnd();
                        }

                        if (!_wpCache.ContainsKey(wpKey))
                            _wpCache.Add(wpKey, result);
                        else
                            _wpCache[wpKey] = result;
                    }
                }
            }

            if (!string.IsNullOrEmpty(webPartModel.WebpartType))
                throw new Exception("WebpartType is not supported yet.");

            if (!string.IsNullOrEmpty(webPartModel.WebpartXmlTemplate))
                result = webPartModel.WebpartXmlTemplate;

            return result;
        }

        protected virtual string ProcessCommonWebpartProperties(string webPartXml, WebPartDefinitionBase webPartModel)
        {
            return WebpartXmlExtensions.LoadWebpartXmlDocument(webPartXml)
                                               .SetTitle(webPartModel.Title)
                                               .SetID(webPartModel.Id)
                                               .ToString();
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var webPartModel = model.WithAssertAndCast<WebPartDefinitionBase>("model", value => value.RequireNotNull());

            var listItem = listItemModelHost.HostListItem;

            var context = listItem.Context;
            var currentPageFile = GetCurrentPageFile(listItemModelHost);

            ModuleFileModelHandler.WithSafeFileOperation(listItem.ParentList, currentPageFile, pageFile =>
            {
                //var fileContext = pageFile.Context;
                var fileListItem = pageFile.ListItemAllFields;
                var fileContext = pageFile.Context;

                fileContext.Load(fileListItem);

                fileContext.ExecuteQuery();

                var webPartManager = pageFile.GetLimitedWebPartManager(PersonalizationScope.Shared);

                // web part on the page
                var webpartOnPage = webPartManager.WebParts.Include(wp => wp.Id, wp => wp.WebPart);
                var webPartDefenitions = context.LoadQuery(webpartOnPage);

                context.ExecuteQuery();

                Microsoft.SharePoint.Client.WebParts.WebPartDefinition wpDefinition;

                WebPart existingWebPart = null;

                // TODO
                var tmpWp = FindExistingWebPart(webPartDefenitions, webPartModel, out wpDefinition);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = existingWebPart,
                    ObjectType = typeof(WebPart),
                    ObjectDefinition = webPartModel,
                    ModelHost = modelHost
                });

                if (wpDefinition != null)
                {
                    wpDefinition.DeleteWebPart();
                    wpDefinition.Context.ExecuteQuery();
                }
                else
                {
                    existingWebPart = tmpWp;
                }

                if (existingWebPart == null)
                {
                    var webPartXML = GetWebpartXmlDefinition(listItemModelHost, webPartModel);
                    webPartXML = ProcessCommonWebpartProperties(webPartXML, webPartModel);

                    // handle wiki page

                    HandleWikiPageProvision(fileListItem, webPartModel);

                    var webPartDefinition = webPartManager.ImportWebPart(webPartXML);
                    var webPartAddedDefinition = webPartManager.AddWebPart(webPartDefinition.WebPart, webPartModel.ZoneId, webPartModel.ZoneIndex);

                    InvokeOnModelEvent<WebPartDefinition, WebPart>(null, ModelEventType.OnUpdating);

                    existingWebPart = webPartDefinition.WebPart;

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = existingWebPart,
                        ObjectType = typeof(WebPart),
                        ObjectDefinition = webPartModel,
                        ModelHost = modelHost
                    });

                    InvokeOnModelEvent<WebPartDefinition, WebPart>(null, ModelEventType.OnUpdated);
                }
                else
                {
                    // BIG TODO for CSOM, delete/export and re import web part?

                    HandleWikiPageProvision(fileListItem, webPartModel);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = existingWebPart,
                        ObjectType = typeof(WebPart),
                        ObjectDefinition = webPartModel,
                        ModelHost = modelHost
                    });
                }

                context.ExecuteQuery();

                return pageFile;
            });
        }

        private void HandleWikiPageProvision(ListItem listItem, WebPartDefinitionBase webpartModel)
        {
            if (!webpartModel.AddToPageContent)
                return;

            var context = listItem.Context;

            var targetFieldName = string.Empty;

            if (listItem.FieldValues.ContainsKey(BuiltInInternalFieldNames.WikiField))
                targetFieldName = BuiltInInternalFieldNames.WikiField;
            else if (listItem.FieldValues.ContainsKey(BuiltInInternalFieldNames.PublishingPageLayout))
                targetFieldName = BuiltInInternalFieldNames.PublishingPageContent;
            else
            {
                return;
            }

            var wikiTemplate = new StringBuilder();

            var wpId = webpartModel.Id
                .Replace("g_", string.Empty)
                .Replace("_", "-");

            var content = listItem[targetFieldName] == null
                ? string.Empty
                : listItem[targetFieldName].ToString();

            wikiTemplate.AppendFormat(
                "​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
            wikiTemplate.AppendFormat("     <div class='ms-rtestate-read {0}' id='div_{0}'>", wpId);
            wikiTemplate.AppendFormat("     </div>");
            wikiTemplate.AppendFormat("</div>");

            var wikiResult = wikiTemplate.ToString();

            if (string.IsNullOrEmpty(content))
            {
                content = wikiResult;

                listItem[targetFieldName] = content;
                listItem.Update();

                context.ExecuteQuery();
            }
            else
            {
                if (content.ToUpper().IndexOf(wpId.ToUpper()) == -1)
                {
                    content += wikiResult;

                    listItem[targetFieldName] = content;
                    listItem.Update();

                    context.ExecuteQuery();
                }
            }
        }

        protected WebPart FindExistingWebPart(
            IEnumerable<Microsoft.SharePoint.Client.WebParts.WebPartDefinition> webPartDefenitions,
            WebPartDefinitionBase webPartModel)
        {
            Microsoft.SharePoint.Client.WebParts.WebPartDefinition wpDefinition = null;

            return FindExistingWebPart(webPartDefenitions, webPartModel, out wpDefinition);
        }

        protected WebPart FindExistingWebPart(IEnumerable<Microsoft.SharePoint.Client.WebParts.WebPartDefinition> webPartDefenitions,
                                              WebPartDefinitionBase webPartModel,
                                              out Microsoft.SharePoint.Client.WebParts.WebPartDefinition wpDefinition)
        {
            wpDefinition = null;

            // gosh, you got to be kidding
            // internally, SharePoint returns StorageKey as ID. hence.. no ability to trace unique web part on the page
            // the only thing is comparing Titles an utilize them as a primary key

            foreach (var webPartDefinition in webPartDefenitions)
            {
                if (String.Compare(webPartDefinition.WebPart.Title, webPartModel.Title, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    wpDefinition = webPartDefinition;

                    return webPartDefinition.WebPart;
                }
            }

            return null;
        }

        #endregion
    }
}
