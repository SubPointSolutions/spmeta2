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
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;
using WebPartDefinition = SPMeta2.Definitions.WebPartDefinition;
using System.Xml;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Exceptions;
using System.Text;
using System.IO;

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

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var webPartModel = model.WithAssertAndCast<WebPartDefinition>("model", value => value.RequireNotNull());

            var listItem = listItemModelHost.HostListItem;

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

            if (existingWebPart == null)
            {
                string webPartXML = string.Empty;

                // TODO
                // another one 'GOSH' as xml has to be used to add web parts
                // wll be replaced with dynamic xml generation via behaviours later and putting the needed type of the webpart inside
                // this is really PITA

                // extracting class name
                // Microsoft.SharePoint.WebPartPages.ContentEditorWebPart, Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c
                if (!string.IsNullOrEmpty(webPartModel.WebpartFileName))
                {
                    var rootWeb = listItemModelHost.HostSite.RootWeb;
                    var rootWebContext = rootWeb.Context;

                    var webPartCatalog = rootWeb.QueryAndGetListByUrl(BuiltInListDefinitions.Calalogs.Wp.GetListUrl());
                    var webParts = webPartCatalog.GetItems(CamlQuery.CreateAllItemsQuery());

                    rootWebContext.Load(webParts);
                    rootWebContext.ExecuteQuery();

                    ListItem targetWebPart = null;

                    foreach (var webPart in webParts)
                        if (webPart["FileLeafRef"].ToString().ToUpper() == webPartModel.WebpartFileName.ToUpper())
                            targetWebPart = webPart;

                    if (targetWebPart == null)
                        throw new SPMeta2Exception(string.Format("Cannot find web part file by name:[{0}]", webPartModel.WebpartFileName));

                    var webPartFile = targetWebPart.File;

                    rootWebContext.Load(webPartFile);
                    rootWebContext.ExecuteQuery();

                    var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(listItemModelHost.HostClientContext, webPartFile.ServerRelativeUrl);

                    using (var reader = new StreamReader(fileInfo.Stream))
                    {
                        webPartXML = reader.ReadToEnd();
                    }
                }

                if (!string.IsNullOrEmpty(webPartModel.WebpartType))
                    throw new Exception("WebpartType is not supported yet.");

                if (!string.IsNullOrEmpty(webPartModel.WebpartXmlTemplate))
                    webPartXML = webPartModel.WebpartXmlTemplate;

                InvokeOnModelEvent<WebPartDefinition, WebPart>(null, ModelEventType.OnUpdating);


                webPartXML = WebpartXmlExtensions.LoadWebpartXmlDocument(webPartXML)
                                                  .SetTitle(webPartModel.Title)
                                                  .ToString();

                // set common properties as title

                var webPartDefinition = webPartManager.ImportWebPart(webPartXML);
                var webPartAddedDefinition = webPartManager.AddWebPart(webPartDefinition.WebPart, webPartModel.ZoneId, webPartModel.ZoneIndex);

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
        }

        protected WebPart FindExistingWebPart(IEnumerable<Microsoft.SharePoint.Client.WebParts.WebPartDefinition> webPartDefenitions,
                                              WebPartDefinition webPartModel)
        {
            // gosh, you got to be kidding
            // internally, SharePoint returns StorageKey as ID. hence.. no ability to trace unique web part on the page
            // the only thing is comparing Titles an utilize them as a primary key

            foreach (var webPartDefinition in webPartDefenitions)
            {
                if (String.Compare(webPartDefinition.WebPart.Title, webPartModel.Title, System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return webPartDefinition.WebPart;
                }
            }

            return null;
        }

        #endregion
    }
}
