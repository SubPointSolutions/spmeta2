using System;
using System.Linq;
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
            var wpModel = webPartModel.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());
            var bindContext = LookupBindContext(listItemModelHost, wpModel);

            var wpXml = WebpartXmlExtensions.LoadWebpartXmlDocument(BuiltInWebPartTemplates.XsltListViewWebPart)
                                .SetListName(bindContext.ListId.ToString())
                                .SetListId(bindContext.ListId.ToString())
                                .SetTitleUrl(bindContext.TitleUrl)
                                .SetJSLink(wpModel.JSLink);

            if (wpModel.CacheXslStorage.HasValue)
                wpXml.SetOrUpdateProperty("CacheXslStorage", wpModel.CacheXslStorage.Value.ToString());

            if (wpModel.CacheXslTimeOut.HasValue)
                wpXml.SetOrUpdateProperty("CacheXslTimeOut", wpModel.CacheXslTimeOut.Value.ToString());

            if (!string.IsNullOrEmpty(wpModel.BaseXsltHashKey))
                wpXml.SetOrUpdateProperty("BaseXsltHashKey", wpModel.BaseXsltHashKey);

            // xsl
            if (!string.IsNullOrEmpty(wpModel.Xsl))
                wpXml.SetOrUpdateCDataProperty("Xsl", wpModel.Xsl);

            if (!string.IsNullOrEmpty(wpModel.XslLink))
                wpXml.SetOrUpdateProperty("XslLink", wpModel.XslLink);

            if (!string.IsNullOrEmpty(wpModel.GhostedXslLink))
                wpXml.SetOrUpdateProperty("GhostedXslLink", wpModel.GhostedXslLink);

            // xml
            if (!string.IsNullOrEmpty(wpModel.XmlDefinition))
                wpXml.SetOrUpdateCDataProperty("XmlDefinition", wpModel.Xsl);

            if (!string.IsNullOrEmpty(wpModel.XmlDefinitionLink))
                wpXml.SetOrUpdateProperty("XmlDefinitionLink", wpModel.XmlDefinitionLink);

#if !NET35
            if (wpModel.ShowTimelineIfAvailable.HasValue)
                wpXml.SetOrUpdateProperty("ShowTimelineIfAvailable", wpModel.ShowTimelineIfAvailable.Value.ToString());
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
        //    var wpModel = webPartModel.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());

        //    var webPartStoreKey = provisionContext.WebPartStoreKey;
        //    var context = provisionContext.ListItemModelHost.HostWeb.Context;

        //    var bindContext = LookupBindContext(listItemModelHost, wpModel);

        //    if (bindContext.TargetViewId.HasValue
        //        && bindContext.TargetViewId != default(Guid)
        //        && provisionContext.WebPartStoreKey.HasValue
        //        && provisionContext.WebPartStoreKey.Value != default(Guid))
        //    {
        //        var targetWeb = listItemModelHost.HostWeb;

        //        if (wpModel.WebId.HasGuidValue() || !string.IsNullOrEmpty(wpModel.WebUrl))
        //        {
        //            targetWeb = new LookupFieldModelHandler()
        //                            .GetTargetWeb(this.CurrentClientContext.Site, wpModel.WebUrl, wpModel.WebId);
        //        }

        //        var list = LookupList(targetWeb, wpModel.ListUrl, wpModel.ListTitle, wpModel.WebId);

        //        var srcView = list.Views.GetById(bindContext.TargetViewId.Value);
        //        var hiddenView = list.Views.GetById(provisionContext.WebPartStoreKey.Value);

        //        context.Load(srcView, s => s.ViewFields);
        //        context.Load(srcView, s => s.HtmlSchemaXml);

        //        context.Load(srcView, s => s.RowLimit);
        //        context.Load(srcView, s => s.ViewQuery);
        //        context.Load(srcView, s => s.JSLink);

        //        context.Load(srcView, s => s.IncludeRootFolder);
        //        context.Load(srcView, s => s.Scope);

        //        context.Load(hiddenView);

        //        context.ExecuteQueryWithTrace();

        //        // patching schema
        //        // first update via ListViewXml doubles the View node

        //        // the name should be replaces with the hidden view name
        //        var hiddenViewXml = XDocument.Parse(hiddenView.HtmlSchemaXml);
        //        var srcViewXml = XDocument.Parse(srcView.HtmlSchemaXml);

        //        var hiddenViewName = hiddenViewXml.Root.GetAttributeValue("Name");
        //        srcViewXml.Root.SetAttributeValue("Name", hiddenViewName);

        //        hiddenView.ListViewXml = srcViewXml.ToString();
        //        hiddenView.Update();
        //        context.ExecuteQueryWithTrace();

        //        // getting again, patching thew view and the rest of the props
        //        hiddenView = list.Views.GetById(provisionContext.WebPartStoreKey.Value);
        //        context.Load(hiddenView);
        //        context.ExecuteQueryWithTrace();

        //        // patching, removing the root node
        //        hiddenView.ListViewXml = srcViewXml.Root.GetInnerXmlAsString();
        //        hiddenView.Update();
        //        context.ExecuteQueryWithTrace();

        //        hiddenView = list.Views.GetById(provisionContext.WebPartStoreKey.Value);
        //        context.Load(hiddenView);
        //        context.Load(hiddenView, v => v.ListViewXml);
        //        context.ExecuteQueryWithTrace();


        //        var srcType = XDocument.Parse(hiddenView.ListViewXml).Root.GetAttributeValue("Type");
        //        if (srcType != "GRID")
        //        {
        //            Console.Write("");
        //        }

        //        // fixing the rest, but why?
        //        // already pushed everything via ListViewXml
        //        // tests are fine, so leave it as it is

        //        //hiddenView.ViewFields.RemoveAll();
        //        //foreach (var f in srcView.ViewFields)
        //        //    hiddenView.ViewFields.Add(f);

        //        //hiddenView.RowLimit = srcView.RowLimit;
        //        //hiddenView.ViewQuery = srcView.ViewQuery;
        //        //hiddenView.JSLink = srcView.JSLink;

        //        //hiddenView.IncludeRootFolder = srcView.IncludeRootFolder;
        //        //hiddenView.Scope = srcView.Scope;

        //        //hiddenView.Update();
        //        //context.ExecuteQueryWithTrace();
        //    }
        //}

        private ListBindContext LookupBindContext(ListItemModelHost listItemModelHost, XsltListViewWebPartDefinition wpModel)
        {
            var result = new ListBindContext
            {

            };

            var web = listItemModelHost.HostWeb;
            var context = listItemModelHost.HostWeb.Context;

            var list = LookupList(listItemModelHost, wpModel);

            View view = null;

            if (wpModel.ViewId.HasValue && wpModel.ViewId != default(Guid))
                view = list.Views.GetById(wpModel.ViewId.Value);
            else if (!string.IsNullOrEmpty(wpModel.ViewName))
                view = list.Views.GetByTitle(wpModel.ViewName);

            context.Load(list, l => l.Id);
            context.Load(list, l => l.DefaultViewUrl);
            context.Load(list, l => l.Title);
            context.Load(list, l => l.DefaultView);

            if (view != null)
            {
                context.Load(view);
                context.ExecuteQueryWithTrace();

                result.OriginalView = list.DefaultView;
                result.OriginalViewId = list.DefaultView.Id;

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

            if (wpModel.TitleUrl == null)
            {
                if (string.IsNullOrEmpty(result.TitleUrl))
                    result.TitleUrl = list.DefaultViewUrl;
            }

            result.DefaultViewId = list.DefaultView.Id;

            return result;
        }

        #endregion
    }
}
