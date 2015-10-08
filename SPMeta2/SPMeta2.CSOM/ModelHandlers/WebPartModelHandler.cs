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
using SPMeta2.Services;
using SPMeta2.Utils;
using WebPartDefinition = SPMeta2.Definitions.WebPartDefinition;
using System.Xml;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Exceptions;
using System.Text;
using System.IO;
using System.Security;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Services.Webparts;
using File = Microsoft.SharePoint.Client.File;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class WebPartModelHandler : CSOMModelHandlerBase
    {
        #region contructors

        public WebPartModelHandler()
        {
            WebPartChromeTypesConvertService = ServiceContainer.Instance.GetService<WebPartChromeTypesConvertService>();
        }

        #endregion

        #region properties

        protected WebPartChromeTypesConvertService WebPartChromeTypesConvertService { get; set; }

        public override Type TargetType
        {
            get { return typeof(WebPartDefinition); }
        }

        // TODO, depends on SP version

        #endregion

        #region classes

        protected class WebPartProcessingContext
        {
            public Guid? WebPartStoreKey { get; set; }
            public ListItemModelHost ListItemModelHost { get; set; }
            public WebPartDefinitionBase WebPartDefinition { get; set; }
        }

        #endregion

        #region methods

        //protected void WithExistingWebPart(ListItem listItem, WebPartDefinition webPartModel,
        //  Action<WebPart> action)
        //{
        //    WithExistingWebPart(listItem.File, webPartModel, action);
        //}

        protected void WithExistingWebPart(File file, WebPartDefinition webPartModel,
           Action<WebPart> action)
        {
            WithExistingWebPart(file, webPartModel, (w, d) =>
            {
                action(w);
            });
        }

        //protected void WithExistingWebPart(ListItem listItem, WebPartDefinition webPartModel,
        //    Action<WebPart, Microsoft.SharePoint.Client.WebParts.WebPartDefinition> action)
        //{
        //    WithExistingWebPart(listItem.File, webPartModel, action);
        //}



        protected void WithExistingWebPart(File pageFile, WebPartDefinition webPartModel,
             Action<WebPart, Microsoft.SharePoint.Client.WebParts.WebPartDefinition> action)
        {
            var context = pageFile.Context;
            var webPartManager = pageFile.GetLimitedWebPartManager(PersonalizationScope.Shared);

            // web part on the page
            var webpartOnPage = webPartManager.WebParts.Include(wp => wp.Id, wp => wp.WebPart);
            var webPartDefenitions = context.LoadQuery(webpartOnPage);

            context.ExecuteQueryWithTrace();

            Microsoft.SharePoint.Client.WebParts.WebPartDefinition def = null;
            var existingWebPart = FindExistingWebPart(webPartDefenitions, webPartModel, out def);

            action(existingWebPart, def);
        }

        protected File GetCurrentPageFile(ListItemModelHost listItemModelHost)
        {
            if (listItemModelHost.HostFile != null)
                return listItemModelHost.HostFile;

            return listItemModelHost.HostListItem.File;
        }

        protected ListItem SearchItemByName(List list, Folder folder, string pageName)
        {
            var context = list.Context;

            if (folder != null)
            {
                if (!folder.IsPropertyAvailable("ServerRelativeUrl"))
                {
                    folder.Context.Load(folder, f => f.ServerRelativeUrl);
                    folder.Context.ExecuteQueryWithTrace();
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
            context.ExecuteQueryWithTrace();

            return collListItems.FirstOrDefault();

        }

        private static Dictionary<string, string> _wpCache = new Dictionary<string, string>();
        private static object _wpCacheLock = new object();

        protected virtual string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var result = string.Empty;
            var context = listItemModelHost.HostList.Context;

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

#pragma warning disable 618
                        var webPartCatalog = rootWeb.QueryAndGetListByUrl(BuiltInListDefinitions.Catalogs.Wp.GetListUrl());
#pragma warning restore 618
                        //var webParts = webPartCatalog.GetItems(CamlQuery.CreateAllItemsQuery());

                        //rootWebContext.Load(webParts);
                        //rootWebcontext.ExecuteQueryWithTrace();

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
                        rootWebContext.ExecuteQueryWithTrace();


                        // does not work for apps - https://github.com/SubPointSolutions/spmeta2/issues/174
                        //var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(listItemModelHost.HostclientContext, webPartFile.ServerRelativeUrl);

                        //using (var reader = new StreamReader(fileInfo.Stream))
                        //{
                        //    webPartXML = reader.ReadToEnd();
                        //}

                        var data = webPartFile.OpenBinaryStream();

                        context.Load(webPartFile);
                        context.ExecuteQueryWithTrace();

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

        protected virtual string ProcessCommonWebpartProperties(string webPartXml, WebPartDefinitionBase definition)
        {
            var xml = WebpartXmlExtensions.LoadWebpartXmlDocument(webPartXml)
                                            .SetTitle(definition.Title)
                                            .SetID(definition.Id);

            if (definition.Width.HasValue)
                xml.SetWidth(definition.Width.Value);

            if (definition.Height.HasValue)
                xml.SetHeight(definition.Height.Value);

            if (!string.IsNullOrEmpty(definition.Description))
                xml.SetDescription(definition.Description);

            if (!string.IsNullOrEmpty(definition.ImportErrorMessage))
                xml.SetImportErrorMessage(definition.ImportErrorMessage);

            if (!string.IsNullOrEmpty(definition.TitleUrl))
            {
                var urlValue = definition.TitleUrl ?? string.Empty;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original value: [{0}]", urlValue);

                urlValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = urlValue,
                    Context = CurrentClientContext
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced value: [{0}]", urlValue);

                xml.SetTitleUrl(urlValue);
            }

            if (!string.IsNullOrEmpty(definition.TitleIconImageUrl))
                xml.SetTitleIconImageUrl(definition.TitleIconImageUrl);

            if (!string.IsNullOrEmpty(definition.ChromeState))
                xml.SetChromeState(definition.ChromeState);

            if (!string.IsNullOrEmpty(definition.ChromeType))
            {
                var chromeType = string.Empty;

                if (xml.IsV3version())
                    chromeType = WebPartChromeTypesConvertService.NormilizeValueToPartChromeTypes(definition.ChromeType);
                else if (xml.IsV2version())
                    chromeType = WebPartChromeTypesConvertService.NormilizeValueToFrameTypes(definition.ChromeType);

                // SetChromeType() sets correct XML props depending on V2/V3 web part XML
                xml.SetChromeType(chromeType);
            }

            if (!string.IsNullOrEmpty(definition.ExportMode))
                xml.SetExportMode(definition.ExportMode);

            // bindings
            ProcessParameterBindings(definition, xml);

            // properties
            ProcessWebpartProperties(definition, xml);


            return xml.ToString();
        }

        protected virtual void ProcessWebpartProperties(WebPartDefinitionBase definition, XDocument xml)
        {
            if (definition.Properties != null && definition.Properties.Count > 0)
            {
                foreach (var prop in definition.Properties)
                {
                    var isCdata = prop.IsCData.HasValue && prop.IsCData.Value;
                    xml.SetOrUpdateProperty(prop.Name, prop.Value, prop.Type, isCdata);
                }
            }
        }

        protected virtual void ProcessParameterBindings(WebPartDefinitionBase definition, XDocument xml)
        {
            if (definition.ParameterBindings != null && definition.ParameterBindings.Count > 0)
            {
                var parameterBinder = new WebPartParameterBindingsOptions();

                foreach (var binding in definition.ParameterBindings)
                    parameterBinder.AddParameterBinding(binding.Name, binding.Location);

                var parameterBindingValue = SecurityElement.Escape(parameterBinder.ParameterBinding);
                xml.SetOrUpdateProperty("ParameterBindings", parameterBindingValue);
            }
        }


        protected ClientContext CurrentClientContext { get; set; }

        protected virtual void OnBeforeDeploy(ListItemModelHost host, WebPartDefinitionBase webpart)
        {

        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            Guid? OldWebParKey = null;

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var webPartModel = model.WithAssertAndCast<WebPartDefinitionBase>("model", value => value.RequireNotNull());

            OnBeforeDeploy(listItemModelHost, webPartModel);

            CurrentClientContext = listItemModelHost.HostClientContext;

            var context = listItemModelHost.HostClientContext;
            var currentPageFile = GetCurrentPageFile(listItemModelHost);


            if (listItemModelHost.HostFolder != null)
            {
                if (!listItemModelHost.HostFolder.IsPropertyAvailable("Properties") ||
                    listItemModelHost.HostFolder.Properties.FieldValues.Count == 0)
                {
                    listItemModelHost.HostFolder.Context.Load(listItemModelHost.HostFolder, f => f.Properties);
                    //folder.Context.Load(folder, f => f.Properties);

                    listItemModelHost.HostFolder.Context.ExecuteQueryWithTrace();
                }
            }

            // TODO
            var doesFileHasListItem =
                //Forms folders
               !(listItemModelHost.HostFolder != null
                &&
                (listItemModelHost.HostFolder.Properties.FieldValues.ContainsKey("vti_winfileattribs")
                 && listItemModelHost.HostFolder.Properties.FieldValues["vti_winfileattribs"].ToString() == "00000012"));

            ModuleFileModelHandler.WithSafeFileOperation(listItemModelHost.HostList,
                currentPageFile, pageFile =>
            {
                Guid? webPartStoreKey = null;

                InternalOnBeforeWebPartProvision(new WebPartProcessingContext
                {
                    ListItemModelHost = listItemModelHost,
                    WebPartDefinition = webPartModel,
                    WebPartStoreKey = webPartStoreKey
                });

                //var fileContext = pageFile.Context;
                ListItem fileListItem = null;

                if (webPartModel.AddToPageContent)
                {
                    // pre load here to be used later

                    var fileContext = pageFile.Context;

                    fileListItem = pageFile.ListItemAllFields;

                    fileContext.Load(fileListItem);
                    fileContext.ExecuteQueryWithTrace();
                }

                var webPartManager = pageFile.GetLimitedWebPartManager(PersonalizationScope.Shared);

                // web part on the page
                var webpartOnPage = webPartManager.WebParts.Include(wp => wp.Id, wp => wp.WebPart);
                var webPartDefenitions = context.LoadQuery(webpartOnPage);

                context.ExecuteQueryWithTrace();

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
                    OldWebParKey = wpDefinition.Id;

                    TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject,
                        "Deleting current web part.");

                    wpDefinition.DeleteWebPart();
                    wpDefinition.Context.ExecuteQueryWithTrace();
                }
                else
                {
                    existingWebPart = tmpWp;
                }

                Microsoft.SharePoint.Client.WebParts.WebPartDefinition webPartAddedDefinition = null;

                if (existingWebPart == null)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject,
                        "Processing new web part");

                    var webPartXML = GetWebpartXmlDefinition(listItemModelHost, webPartModel);
                    webPartXML = ProcessCommonWebpartProperties(webPartXML, webPartModel);

                    //// handle wiki page
                    //if (webPartModel.AddToPageContent)
                    //{

                    //    HandleWikiPageProvision(fileListItem, webPartModel);
                    //}

                    var webPartDefinition = webPartManager.ImportWebPart(webPartXML);
                    webPartAddedDefinition = webPartManager.AddWebPart(webPartDefinition.WebPart,
                                                                       webPartModel.ZoneId,
                                                                       webPartModel.ZoneIndex);

                    context.Load(webPartAddedDefinition);
                    context.ExecuteQueryWithTrace();

                    if (webPartAddedDefinition != null && webPartAddedDefinition.ServerObjectIsNull == false)
                    {
                        existingWebPart = webPartAddedDefinition.WebPart;
                        webPartStoreKey = webPartAddedDefinition.Id;
                    }

                    // handle wiki page
                    if (webPartModel.AddToPageContent)
                    {
                        HandleWikiPageProvision(fileListItem, webPartModel, webPartStoreKey, OldWebParKey);
                    }

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
                }
                else
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject,
                        "Processing existing web part");

                    if (webPartModel.AddToPageContent)
                    {
                        //HandleWikiPageProvision(fileListItem, webPartModel);
                    }

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

                context.ExecuteQueryWithTrace();

                if (webPartAddedDefinition != null && webPartAddedDefinition.ServerObjectIsNull == false)
                {
                    existingWebPart = webPartAddedDefinition.WebPart;
                    webPartStoreKey = webPartAddedDefinition.Id;
                }

                InternalOnAfterWebPartProvision(new WebPartProcessingContext
                {
                    ListItemModelHost = listItemModelHost,
                    WebPartDefinition = webPartModel,
                    WebPartStoreKey = webPartStoreKey
                });

                return pageFile;
            }, doesFileHasListItem);
        }

        protected virtual void InternalOnBeforeWebPartProvision(WebPartProcessingContext context)
        {

        }

        protected virtual void InternalOnAfterWebPartProvision(WebPartProcessingContext context)
        {

        }

        private void HandleWikiPageProvision(ListItem listItem,
            WebPartDefinitionBase webpartModel, Guid? webPartStoreKey, Guid? oldWebParKey)
        {
            if (!webpartModel.AddToPageContent)
                return;

            TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "AddToPageContent = true. Handling wiki/publishig page provision case.");

            var context = listItem.Context;

            var targetFieldName = string.Empty;

            if (listItem.FieldValues.ContainsKey(BuiltInInternalFieldNames.WikiField))
            {
                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "WikiField field is detected. Switching to wiki page web part provision.");

                targetFieldName = BuiltInInternalFieldNames.WikiField;
            }
            else if (listItem.FieldValues.ContainsKey(BuiltInInternalFieldNames.PublishingPageLayout))
            {
                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "PublishingPageLayout field is detected. Switching to publishin page web part provision.");

                targetFieldName = BuiltInInternalFieldNames.PublishingPageContent;
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "Not PublishingPageLayout field, nor WikiField is detected. Skipping.");
                return;
            }

            var wikiTemplate = new StringBuilder();

            var existingWebPartId = string.Empty;

            var definitionWebPartId = webpartModel.Id.ToString()
                                      .Replace("g_", string.Empty)
                                      .Replace("_", "-"); ;

            var upcomingWebPartId = definitionWebPartId;

            // aa....
            // extremely unfortunate 
            if (webpartModel is XsltListViewWebPartDefinition)
            {
                upcomingWebPartId = webPartStoreKey.ToString()
                                      .Replace("g_", string.Empty)
                                      .Replace("_", "-"); ;
            }

            if (!oldWebParKey.HasGuidValue())
            {
                // first provision
                existingWebPartId = webPartStoreKey.ToString()
                                      .Replace("g_", string.Empty)
                                      .Replace("_", "-");
            }
            else
            {
                // second, so that we had web part and use that ID
                existingWebPartId = oldWebParKey.ToString()
                                      .Replace("g_", string.Empty)
                                      .Replace("_", "-");
            }

            var content = listItem[targetFieldName] == null
                ? string.Empty
                : listItem[targetFieldName].ToString();

            // actual ID will be replaced later
            wikiTemplate.AppendFormat("​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
            wikiTemplate.Append("     <div class='ms-rtestate-read {0}' id='div_{0}'>");
            wikiTemplate.AppendFormat("     </div>");
            wikiTemplate.AppendFormat("</div>");

            var wikiTemplateOutput = wikiTemplate.ToString();

            if (string.IsNullOrEmpty(content))
            {
                // page is empty, pre-generating HTML
                // pushing web part as the current WebPart Key
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                    "Page content is empty Generating new one.");

                content = string.Format(wikiTemplateOutput, upcomingWebPartId);

                listItem[targetFieldName] = content;
                listItem.Update();

                context.ExecuteQueryWithTrace();
            }
            else
            {
                // we had web part on the page
                // so we need to change the ID
                if (oldWebParKey.HasGuidValue())
                {
                    // was old on the page?
                    if (content.ToUpper().IndexOf(existingWebPartId.ToUpper()) != -1)
                    {
                        // yes, replacing ID
                        TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                            string.Format("Replacing web part with ID: [{0}] to [{1}] on the page content.",
                                existingWebPartId, upcomingWebPartId),
                            null);

                        content = content.Replace(existingWebPartId, upcomingWebPartId);

                        listItem[targetFieldName] = content;
                        listItem.Update();

                        context.ExecuteQueryWithTrace();
                    }
                    // original from the definigion?
                    else if (content.ToUpper().IndexOf(definitionWebPartId.ToUpper()) != -1)
                    {
                        // yes, replacing ID
                        TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                            string.Format("Replacing web part with ID: [{0}] to [{1}] on the page content.",
                                existingWebPartId, upcomingWebPartId),
                            null);

                        content = content.Replace(definitionWebPartId, upcomingWebPartId);

                        listItem[targetFieldName] = content;
                        listItem.Update();

                        context.ExecuteQueryWithTrace();
                    }
                    else
                    {
                        // adding new, from scratch 
                        TraceService.WarningFormat((int)LogEventId.ModelProvisionCoreCall,
                            "Cannot find web part ID: [{0}] the page content. Adding a new one.",
                            new object[]
                            {
                                existingWebPartId
                            });

                        content = string.Format(wikiTemplateOutput, upcomingWebPartId);

                        listItem[targetFieldName] = content;
                        listItem.Update();

                        context.ExecuteQueryWithTrace();
                    }
                }
                else
                {
                    // first provision, no web parts on the wiki page

                    // there should be a definition based web part id in the template
                    // updatting to a upcoming ID
                    if (content.ToUpper().IndexOf(definitionWebPartId.ToUpper()) != -1)
                    {
                        // yes, replacing ID
                        TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                            string.Format("Replacing web part with ID: [{0}] to [{1}] on the page content.",
                                definitionWebPartId, upcomingWebPartId),
                            null);

                        content = content.Replace(definitionWebPartId, upcomingWebPartId);

                        listItem[targetFieldName] = content;
                        listItem.Update();

                        context.ExecuteQueryWithTrace();
                    }
                    else
                    {
                        // adding new, from scratch 
                        TraceService.WarningFormat((int)LogEventId.ModelProvisionCoreCall,
                            "Cannot find web part ID: [{0}] the page content. Adding a new one.",
                            new object[]
                            {
                                existingWebPartId
                            });

                        content = string.Format(wikiTemplateOutput, upcomingWebPartId);

                        listItem[targetFieldName] = content;
                        listItem.Update();

                        context.ExecuteQueryWithTrace();
                    }
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
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving web part by Title: [{0}]", webPartModel.Title);

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
