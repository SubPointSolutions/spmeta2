using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class ListViewWebPartModelHandler : WebPartModelHandler
    {
        #region classes

        internal class ListBindContext
        {
            public View OriginalView { get; set; }
            public View TargetView { get; set; }

            public Guid? OriginalViewId { get; set; }
            public Guid? TargetViewId { get; set; }

            public Guid ListId { get; set; }
            public string TitleUrl { get; set; }
        }


        #endregion

        #region properties

        public override Type TargetType
        {
            get { return typeof(ListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var wpModel = webPartModel.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var bindContext = LookupBindContext(listItemModelHost, wpModel);

            var wpXml = WebpartXmlExtensions.LoadWebpartXmlDocument(BuiltInWebPartTemplates.ListViewWebPart)
                                         .SetOrUpdateListVieweWebPartProperty("ListName", bindContext.ListId.ToString("B"))
                                         .SetOrUpdateListVieweWebPartProperty("ListId", bindContext.ListId.ToString("D").ToLower())
                                         //.SetOrUpdateListVieweWebPartProperty("ViewGuid", bindContext.ViewId.ToString("D").ToLower())
                                         .SetTitleUrl(bindContext.TitleUrl)
                                         .ToString();

            return wpXml;
        }

        protected override void InternalOnAfterWebPartProvision(WebPartProcessingContext provisionContext)
        {
            base.InternalOnAfterWebPartProvision(provisionContext);

            var webPartModel = provisionContext.WebPartDefinition;

            var listItemModelHost = provisionContext.ListItemModelHost;
            var wpModel = webPartModel.WithAssertAndCast<ListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var webPartStoreKey = provisionContext.WebPartStoreKey;
            var context = provisionContext.ListItemModelHost.HostWeb.Context;

            var bindContext = LookupBindContext(listItemModelHost, wpModel);

            if (bindContext.TargetViewId.HasValue
                && bindContext.TargetViewId != default(Guid)
                && provisionContext.WebPartStoreKey.HasValue
                && provisionContext.WebPartStoreKey.Value != default(Guid))
            {
                var list = LookupList(listItemModelHost, wpModel);

                var srcView = list.Views.GetById(bindContext.TargetViewId.Value);
                var hiddenView = list.Views.GetById(provisionContext.WebPartStoreKey.Value);

                context.Load(srcView, s => s.ViewFields);

                context.Load(srcView, s => s.RowLimit);
                context.Load(srcView, s => s.ViewQuery);
                context.Load(srcView, s => s.JSLink);

                context.Load(srcView, s => s.IncludeRootFolder);
                context.Load(srcView, s => s.Scope);

                context.Load(hiddenView);

                context.ExecuteQuery();

                hiddenView.ViewFields.RemoveAll();

                foreach (var f in srcView.ViewFields)
                    hiddenView.ViewFields.Add(f);


                hiddenView.RowLimit = srcView.RowLimit;
                hiddenView.ViewQuery = srcView.ViewQuery;
                hiddenView.JSLink = srcView.JSLink;

                hiddenView.IncludeRootFolder = srcView.IncludeRootFolder;
                hiddenView.Scope = srcView.Scope;

                hiddenView.Update();
                context.ExecuteQuery();
            }
        }

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


        private ListBindContext LookupBindContext(ListItemModelHost listItemModelHost, ListViewWebPartDefinition wpModel)
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

            if (wpModel.TitleUrl == null)
            {
                if (string.IsNullOrEmpty(result.TitleUrl))
                    result.TitleUrl = list.DefaultViewUrl;
            }

            return result;
        }

        #endregion
    }
}
