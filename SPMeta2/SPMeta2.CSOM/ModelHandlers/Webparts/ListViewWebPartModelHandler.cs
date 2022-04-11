using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHandlers.Fields;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class ListViewWebPartModelHandler : WebPartModelHandler
    {
        #region classes

        #endregion

        #region properties

        public override Type TargetType
        {
            get { return typeof(ListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override void OnBeforeDeploy(ListItemModelHost host, WebPartDefinitionBase webpart)
        {
            base.OnBeforeDeploy(host, webpart);

            // pre-load web id
            if (!host.HostWeb.IsPropertyAvailable("Id"))
            {
                host.HostWeb.Context.Load(host.HostWeb, w => w.Id);
                host.HostWeb.Context.ExecuteQueryWithTrace();
            }

            var context = host.HostClientContext;
            var wpModel = webpart.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

            // save the old default view ID, then restore in OnAfterDeploy
            _currentListBindContext = XsltListViewWebPartModelHandler.LookupBindContext(host,
                                    wpModel.WebUrl, wpModel.WebId,
                                    wpModel.ListUrl, wpModel.ListTitle, wpModel.ListId,
                                    wpModel.ViewUrl, wpModel.ViewName, wpModel.ViewId,
                                    wpModel.TitleUrl);

            if (_currentListBindContext.TargetView != null)
            {
                _currentListBindContext.TargetView.DefaultView = true;
                _currentListBindContext.TargetView.Update();

                context.ExecuteQueryWithTrace();
            }
        }

        private XsltListViewWebPartModelHandler.ListBindContext _currentListBindContext;


        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var wpModel = webPartModel.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var bindContext = XsltListViewWebPartModelHandler.LookupBindContext(listItemModelHost,
                                    wpModel.WebUrl, wpModel.WebId,
                                    wpModel.ListUrl, wpModel.ListTitle, wpModel.ListId,
                                    wpModel.ViewUrl, wpModel.ViewName, wpModel.ViewId,
                                    wpModel.TitleUrl);

            var webId = listItemModelHost.HostWeb.Id;

            var wpXml = WebpartXmlExtensions.LoadWebpartXmlDocument(BuiltInWebPartTemplates.ListViewWebPart)
                                         .SetOrUpdateListVieweWebPartProperty("ListName", bindContext.ListId.ToString("B"))
                                         .SetOrUpdateListVieweWebPartProperty("ListId", bindContext.ListId.ToString("D").ToLower())
                                         .SetOrUpdateListVieweWebPartProperty("WebId", webId.ToString("D").ToLower())
                //.SetOrUpdateListVieweWebPartProperty("ViewGuid", bindContext.ViewId.ToString("D").ToLower())
                                         .SetTitleUrl(bindContext.TitleUrl)
                                         .ToString();

            return wpXml;
        }


        protected override void OnAfterDeploy(ListItemModelHost host, WebPartDefinitionBase webpart)
        {
            if (_currentListBindContext != null)
            {
                var context = host.HostClientContext;
                var wpModel = webpart.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

                var bindingContext = XsltListViewWebPartModelHandler.LookupBindContext(host,
                                    wpModel.WebUrl, wpModel.WebId,
                                    wpModel.ListUrl, wpModel.ListTitle, wpModel.ListId,
                                    wpModel.ViewUrl, wpModel.ViewName, wpModel.ViewId,
                                    wpModel.TitleUrl);

                // reverting back the dafult view
                var view = bindingContext.List.GetView(_currentListBindContext.DefaultViewId);
                view.DefaultView = true;
                view.Update();

                context.ExecuteQueryWithTrace();
            }
        }

        //protected override void InternalOnAfterWebPartProvision(WebPartProcessingContext provisionContext)
        //{
        //    base.InternalOnAfterWebPartProvision(provisionContext);

        //    var webPartModel = provisionContext.WebPartDefinition;

        //    var listItemModelHost = provisionContext.ListItemModelHost;
        //    var wpModel = webPartModel.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

        //    var webPartStoreKey = provisionContext.WebPartStoreKey;
        //    var context = provisionContext.ListItemModelHost.HostWeb.Context;

        //    var bindContext = LookupBindContext(listItemModelHost, wpModel);

        //    if (bindContext.TargetViewId.HasValue
        //        && bindContext.TargetViewId != default(Guid)
        //        && provisionContext.WebPartStoreKey.HasValue
        //        && provisionContext.WebPartStoreKey.Value != default(Guid))
        //    {
        //        var list = LookupList(listItemModelHost, wpModel);

        //        var srcView = list.Views.GetById(bindContext.TargetViewId.Value);
        //        var hiddenView = list.Views.GetById(provisionContext.WebPartStoreKey.Value);

        //        context.Load(srcView, s => s.ViewFields);

        //        context.Load(srcView, s => s.RowLimit);
        //        context.Load(srcView, s => s.ViewQuery);
        //        context.Load(srcView, s => s.JSLink);

        //        context.Load(srcView, s => s.IncludeRootFolder);
        //        context.Load(srcView, s => s.Scope);

        //        context.Load(hiddenView);

        //        context.ExecuteQueryWithTrace();

        //        hiddenView.ViewFields.RemoveAll();

        //        foreach (var f in srcView.ViewFields)
        //            hiddenView.ViewFields.Add(f);


        //        hiddenView.RowLimit = srcView.RowLimit;
        //        hiddenView.ViewQuery = srcView.ViewQuery;
        //        hiddenView.JSLink = srcView.JSLink;

        //        hiddenView.IncludeRootFolder = srcView.IncludeRootFolder;
        //        hiddenView.Scope = srcView.Scope;

        //        hiddenView.Update();
        //        context.ExecuteQueryWithTrace();
        //    }
        //}

        private List LookupList(ListItemModelHost listItemModelHost, ListViewWebPartDefinition wpModel)
        {
            var web = listItemModelHost.HostWeb;
            var context = listItemModelHost.HostWeb.Context;

            List list = null;

            if (!string.IsNullOrEmpty(wpModel.ListUrl))
            {
                list = web.QueryAndGetListByUrl(wpModel.ListUrl);
            }
            else if (!string.IsNullOrEmpty(wpModel.ListTitle))
            {
                list = web.Lists.GetByTitle(wpModel.ListTitle);
            }
            else if (wpModel.ListId != default(Guid))
            {
                list = web.Lists.GetById(wpModel.ListId.Value);
            }
            else
            {
                throw new SPMeta2Exception("ListUrl, ListTitle or ListId should be defined.");
            }

            return list;
        }


        protected override void InternalOnAfterWebPartProvision(WebPartProcessingContext provisionContext)
        {
            base.InternalOnAfterWebPartProvision(provisionContext);

            var webPartModel = provisionContext.WebPartDefinition;

            var listItemModelHost = provisionContext.ListItemModelHost;
            var typedDefinition = webPartModel.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var webPartStoreKey = provisionContext.WebPartStoreKey;
            var context = provisionContext.ListItemModelHost.HostWeb.Context;

            var bindContext = XsltListViewWebPartModelHandler.LookupBindContext(listItemModelHost,
                                    typedDefinition.WebUrl, typedDefinition.WebId,
                                    typedDefinition.ListUrl, typedDefinition.ListTitle, typedDefinition.ListId,
                                    typedDefinition.ViewUrl, typedDefinition.ViewName, typedDefinition.ViewId,
                                    typedDefinition.TitleUrl);

            if (provisionContext.WebPartStoreKey.HasValue
                && provisionContext.WebPartStoreKey.Value != default(Guid))
            {
                var targetWeb = listItemModelHost.HostWeb;

                if (typedDefinition.WebId.HasGuidValue() || !string.IsNullOrEmpty(typedDefinition.WebUrl))
                {
                    targetWeb = new LookupFieldModelHandler()
                                    .GetTargetWeb(this.CurrentClientContext.Site, 
                                                  typedDefinition.WebUrl, 
                                                  typedDefinition.WebId,
                                                  provisionContext.ListItemModelHost);
                }

                var list = XsltListViewWebPartModelHandler.LookupList(targetWeb, typedDefinition.ListUrl, typedDefinition.ListTitle, typedDefinition.ListId);
                var hiddenView = list.Views.GetById(provisionContext.WebPartStoreKey.Value);

                context.Load(hiddenView, s => s.HtmlSchemaXml);

                context.Load(hiddenView);
                context.ExecuteQueryWithTrace();

                // patching the toolbar value

                if (!string.IsNullOrEmpty(typedDefinition.Toolbar))
                {
                    var htmlSchemaXml = XDocument.Parse(hiddenView.HtmlSchemaXml);

                    var useShowAlwaysValue =
                        (typedDefinition.Toolbar.ToUpper() == BuiltInToolbarType.Standard.ToUpper())
                        && typedDefinition.ToolbarShowAlways.HasValue
                        && typedDefinition.ToolbarShowAlways.Value;

                    var toolbarNode = htmlSchemaXml.Root
                        .Descendants("Toolbar")
                        .FirstOrDefault();

                    if (toolbarNode == null)
                    {
                        toolbarNode = new XElement("Toolbar");
                        htmlSchemaXml.Root.Add(toolbarNode);
                    }

                    toolbarNode.SetAttributeValue("Type", typedDefinition.Toolbar);

                    if (useShowAlwaysValue)
                    {
                        toolbarNode.SetAttributeValue("ShowAlways", "TRUE");
                    }
                    else
                    {
                        XAttribute attr = toolbarNode.Attribute("ShowAlways");
                        if (attr != null && string.IsNullOrEmpty(attr.Value))
                            attr.Remove();
                    }

#if !NET35
                    hiddenView.ListViewXml = htmlSchemaXml.Root.GetInnerXmlAsString();

                    // updating other attribute based properties, in the root node
                    // partly related to following issue
                    // List view scope does not apply in xslt list view webpart #1030
                    // https://github.com/SubPointSolutions/spmeta2/issues/1030

                    var scopeValue = htmlSchemaXml.Root.GetAttributeValue("Scope");

                    if (!string.IsNullOrEmpty(scopeValue))
                    {
                        hiddenView.Scope = (ViewScope)Enum.Parse(typeof(ViewScope), scopeValue);
                    }
#endif

                    hiddenView.Update();
                    context.ExecuteQueryWithTrace();
                }
            }
        }


        #endregion
    }
}
