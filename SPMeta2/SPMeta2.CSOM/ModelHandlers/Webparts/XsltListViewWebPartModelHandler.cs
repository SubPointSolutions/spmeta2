using System;
using System.Linq;
using System.Reflection;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers.Fields;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using System.Threading;
using System.Xml.Linq;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class XsltListViewWebPartModelHandler : WebPartModelHandler
    {
        public XsltListViewWebPartModelHandler()
        {
            ShouldUseWebPartStoreKeyForWikiPage = true;
        }

        #region classes

        internal class ListBindContext
        {
            public View OriginalView { get; set; }
            public View TargetView { get; set; }

            public Guid? OriginalViewId { get; set; }
            public Guid? TargetViewId { get; set; }

            public Guid ListId { get; set; }
            public string TitleUrl { get; set; }

            public Guid DefaultViewId { get; set; }

            public List List { get; set; }

            public Guid WebId { get; set; }
        }

        #endregion

        #region properties

        public override Type TargetType
        {
            get { return typeof(XsltListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var typedModel = webPartModel.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());
            var bindContext = LookupBindContext(listItemModelHost, typedModel);

            var wpXml = WebpartXmlExtensions.LoadWebpartXmlDocument(BuiltInWebPartTemplates.XsltListViewWebPart)
                                .SetListName(bindContext.ListId.ToString())
                                .SetListId(bindContext.ListId.ToString())
                                .SetWebId(bindContext.WebId.ToString())
                                .SetTitleUrl(bindContext.TitleUrl)
                                .SetJSLink(typedModel.JSLink);

            if (typedModel.CacheXslStorage.HasValue)
                wpXml.SetOrUpdateProperty("CacheXslStorage", typedModel.CacheXslStorage.Value.ToString());

            if (typedModel.CacheXslTimeOut.HasValue)
                wpXml.SetOrUpdateProperty("CacheXslTimeOut", typedModel.CacheXslTimeOut.Value.ToString());

            if (!string.IsNullOrEmpty(typedModel.BaseXsltHashKey))
                wpXml.SetOrUpdateProperty("BaseXsltHashKey", typedModel.BaseXsltHashKey);

            // xsl
            if (!string.IsNullOrEmpty(typedModel.Xsl))
                wpXml.SetOrUpdateCDataProperty("Xsl", typedModel.Xsl);

            if (!string.IsNullOrEmpty(typedModel.XslLink))
                wpXml.SetOrUpdateProperty("XslLink", typedModel.XslLink);

            if (!string.IsNullOrEmpty(typedModel.GhostedXslLink))
                wpXml.SetOrUpdateProperty("GhostedXslLink", typedModel.GhostedXslLink);

            // xml
            if (!string.IsNullOrEmpty(typedModel.XmlDefinition))
                wpXml.SetOrUpdateCDataProperty("XmlDefinition", typedModel.XmlDefinition);

            if (!string.IsNullOrEmpty(typedModel.XmlDefinitionLink))
                wpXml.SetOrUpdateProperty("XmlDefinitionLink", typedModel.XmlDefinitionLink);

#if !NET35
            if (typedModel.ShowTimelineIfAvailable.HasValue)
                wpXml.SetOrUpdateProperty("ShowTimelineIfAvailable", typedModel.ShowTimelineIfAvailable.Value.ToString());
#endif

            if (typedModel.DisableColumnFiltering.HasValue)
                wpXml.SetOrUpdateProperty("DisableColumnFiltering", typedModel.DisableColumnFiltering.Value.ToString());

#if !NET35
            if (typedModel.DisableSaveAsNewViewButton.HasValue)
                wpXml.SetOrUpdateProperty("DisableSaveAsNewViewButton", typedModel.DisableSaveAsNewViewButton.Value.ToString());

            if (typedModel.DisableViewSelectorMenu.HasValue)
                wpXml.SetOrUpdateProperty("DisableViewSelectorMenu", typedModel.DisableViewSelectorMenu.Value.ToString());

            if (typedModel.InplaceSearchEnabled.HasValue)
                wpXml.SetOrUpdateProperty("InplaceSearchEnabled", typedModel.InplaceSearchEnabled.Value.ToString());
#endif

            return wpXml.ToString();
        }

        private static List LookupList(ListItemModelHost listItemModelHost, XsltListViewWebPartDefinition wpModel)
        {
            return LookupList(listItemModelHost.HostWeb,
                        wpModel.ListUrl,
                        wpModel.ListTitle,
                        wpModel.ListId);
        }

        public static List LookupList(Web web,
            string listUrl, string listTitle, Guid? listId)
        {
            List list = null;

            if (!string.IsNullOrEmpty(listUrl))
            {
                list = web.QueryAndGetListByUrl(listUrl);
            }
            else if (!string.IsNullOrEmpty(listTitle))
            {
                list = web.Lists.GetByTitle(listTitle);
            }
            else if (listId.HasGuidValue())
            {
                list = web.Lists.GetById(listId.Value);
            }
            else
            {
                throw new SPMeta2Exception("ListUrl, ListTitle or ListId should be defined.");
            }

            return list;
        }

        protected override void OnBeforeDeploy(ListItemModelHost host, WebPartDefinitionBase webpart)
        {
            base.OnBeforeDeploy(host, webpart);

            var context = host.HostClientContext;
            var wpModel = webpart.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());

            // save the old default view ID, then restore in OnAfterDeploy
            _currentListBindContext = LookupBindContext(host, wpModel);

            if (_currentListBindContext.TargetView != null)
            {
                _currentListBindContext.TargetView.DefaultView = true;
                _currentListBindContext.TargetView.Update();

                context.ExecuteQueryWithTrace();
            }
        }

        private ListBindContext _currentListBindContext;

        protected override void OnAfterDeploy(ListItemModelHost host, WebPartDefinitionBase webpart)
        {
            if (_currentListBindContext != null)
            {
                var context = host.HostClientContext;
                var wpModel = webpart.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());

                var bindingContext = LookupBindContext(host, wpModel);

                // reverting back the dafult view
                var view = bindingContext.List.GetView(_currentListBindContext.DefaultViewId);

                view.DefaultView = true;
                view.Update();

                context.ExecuteQueryWithTrace();
            }
        }

        protected override void InternalOnAfterWebPartProvision(WebPartProcessingContext provisionContext)
        {
            base.InternalOnAfterWebPartProvision(provisionContext);

            var webPartModel = provisionContext.WebPartDefinition;

            var listItemModelHost = provisionContext.ListItemModelHost;
            var typedDefinition = webPartModel.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var webPartStoreKey = provisionContext.WebPartStoreKey;
            var context = provisionContext.ListItemModelHost.HostWeb.Context;

            var bindContext = LookupBindContext(listItemModelHost, typedDefinition);

            if (provisionContext.WebPartStoreKey.HasValue
                && provisionContext.WebPartStoreKey.Value != default(Guid))
            {
                var targetWeb = listItemModelHost.HostWeb;

                if (typedDefinition.WebId.HasGuidValue() || !string.IsNullOrEmpty(typedDefinition.WebUrl))
                {
                    var lookupFieldModelHandler = new LookupFieldModelHandler();

                    targetWeb = lookupFieldModelHandler.GetTargetWeb(
                                    this.CurrentClientContext.Site,
                                    typedDefinition.WebUrl,
                                    typedDefinition.WebId,
                                    provisionContext.ListItemModelHost);
                }

                var list = LookupList(targetWeb, typedDefinition.ListUrl, typedDefinition.ListTitle, typedDefinition.ListId);
                var hiddenView = list.Views.GetById(provisionContext.WebPartStoreKey.Value);

                context.Load(hiddenView, s => s.HtmlSchemaXml);

                context.Load(hiddenView);
                context.ExecuteQueryWithTrace();

                // always replace HtmlSchemaXml witjh the real view
                // some properties aren't coming with CSOM

                if (bindContext.OriginalView != null)
                {
                    var updatedSchemaXml = XDocument.Parse(hiddenView.HtmlSchemaXml);
                    var originalSchemaXml = XDocument.Parse(bindContext.OriginalView.HtmlSchemaXml);

                    updatedSchemaXml.Root.ReplaceWith(originalSchemaXml.Root);

#if !NET35
                    // updating inner xml definition for view
                    hiddenView.ListViewXml = updatedSchemaXml.Root.GetInnerXmlAsString();
                    
                    // updating other attribute based properties, in the root node
                    // partly related to following issue
                    // List view scope does not apply in xslt list view webpart #1030
                    // https://github.com/SubPointSolutions/spmeta2/issues/1030

                    var scopeValue = updatedSchemaXml.Root.GetAttributeValue("Scope");

                    if(!string.IsNullOrEmpty(scopeValue))
                    {
                        hiddenView.Scope = (ViewScope)Enum.Parse(typeof(ViewScope), scopeValue);
                    }
#endif
                }

                if (!string.IsNullOrEmpty(typedDefinition.Toolbar))
                {
                    // work with the update schema XML
                    var htmlSchemaXml = XDocument.Parse(hiddenView.HtmlSchemaXml);

                    if (bindContext.OriginalView != null)
                    {
                        htmlSchemaXml = XDocument.Parse(bindContext.OriginalView.HtmlSchemaXml);
                    }

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
#endif
                }

                hiddenView.Update();
                context.ExecuteQueryWithTrace();
            }
        }

        internal static ListBindContext LookupBindContext(ListItemModelHost listItemModelHost,
           XsltListViewWebPartDefinition wpModel)
        {
            return LookupBindContext(listItemModelHost,
                wpModel.WebUrl, wpModel.WebId,
                wpModel.ListUrl, wpModel.ListTitle, wpModel.ListId,
                wpModel.ViewUrl, wpModel.ViewName, wpModel.ViewId,
                wpModel.TitleUrl);
        }

        internal static ListBindContext LookupBindContext(ListItemModelHost listItemModelHost,
           string webUrl, Guid? webId,
           string listUrl, string listTitle, Guid? listId,
           string viewUrl, string viewTitle, Guid? viewId,
            string webPartTitleUrl
            )
        {
            var result = new ListBindContext
            {

            };

            var web = listItemModelHost.HostWeb;
            var context = listItemModelHost.HostWeb.Context;

            var targetWeb = listItemModelHost.HostWeb;

            if (webId.HasGuidValue() || !string.IsNullOrEmpty(webUrl))
            {
                var lookupFieldModelHandler = new LookupFieldModelHandler();

                targetWeb = lookupFieldModelHandler
                                .GetTargetWeb(
                                        listItemModelHost.HostClientContext.Site,
                                        webUrl, 
                                        webId,
                                        listItemModelHost);

                result.WebId = targetWeb.Id;
            }

            var list = LookupList(targetWeb, listUrl, listTitle, listId);

            View view = null;

            if (viewId.HasValue && viewId != default(Guid))
                view = list.Views.GetById(viewId.Value);
            else if (!string.IsNullOrEmpty(viewTitle))
                view = list.Views.GetByTitle(viewTitle);
            else if (!string.IsNullOrEmpty(viewUrl))
            {
                var views = list.Views;

                context.Load(views, v => v.Include(r => r.ServerRelativeUrl));
                context.ExecuteQueryWithTrace();

                view = views.ToArray()
                            .FirstOrDefault(v => v.ServerRelativeUrl.ToUpper().EndsWith(viewUrl.ToUpper()));
            }

            context.Load(list, l => l.Id);
            context.Load(list, l => l.DefaultViewUrl);
            context.Load(list, l => l.Title);

            // TODO, https://github.com/SubPointSolutions/spmeta2/issues/765
            // list.DefaultView is not available, so a full fetch for list view is a must for SP2010.

#if !NET35
            context.Load(list, l => l.DefaultView);
#endif

            if (view != null)
            {
                context.Load(view);
                context.ExecuteQueryWithTrace();

#if !NET35
                result.OriginalView = list.DefaultView;
                result.OriginalViewId = list.DefaultView.Id;
#endif

                result.TargetView = view;
                result.TargetViewId = view.Id;

                result.TitleUrl = view.ServerRelativeUrl;
            }
            else
            {
                context.ExecuteQueryWithTrace();
            }

            result.ListId = list.Id;
            result.List = list;


            result.List = list;

            if (webPartTitleUrl == null)
            {
                if (string.IsNullOrEmpty(result.TitleUrl))
                    result.TitleUrl = list.DefaultViewUrl;
            }

#if !NET35
            result.DefaultViewId = list.DefaultView.Id;
#endif

            return result;
        }

        #endregion
    }
}
