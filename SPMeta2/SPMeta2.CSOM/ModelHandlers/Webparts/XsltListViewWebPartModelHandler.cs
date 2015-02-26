using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class XsltListViewWebPartDefinitionValidator : WebPartModelHandler
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
            get { return typeof(XsltListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        private Guid? _originalViewId;

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var wpModel = webPartModel.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var context = listItemModelHost.HostWeb.Context;
            var bindContext = LookupBindContext(listItemModelHost, wpModel);

            // replace defualt list view
            // it will be reverted within InternalOnAfterWebPartProvision()
            if (bindContext.OriginalViewId.HasValue
                && bindContext.TargetViewId.HasValue
                && bindContext.OriginalViewId.Value != bindContext.TargetViewId.Value)
            {
                bindContext.TargetView.DefaultView = true;
                bindContext.TargetView.Update();

                context.ExecuteQuery();

                _originalViewId = bindContext.OriginalViewId.Value;
            }

            var wpXml = WebpartXmlExtensions.LoadWebpartXmlDocument(BuiltInWebPartTemplates.XsltListViewWebPart)
                                         .SetListName(bindContext.ListId.ToString())
                                         .SetListId(bindContext.ListId.ToString())
                                         .SetTitleUrl(bindContext.TitleUrl)
                                         .SetOrUpdateProperty("JSLink", wpModel.JSLink)
                                         .ToString();

            return wpXml;
        }

        protected override void InternalOnAfterWebPartProvision(ListItemModelHost listItemModelHost, WebPartDefinitionBase wpModel)
        {
            base.InternalOnAfterWebPartProvision(listItemModelHost, wpModel);

            if (_originalViewId.HasValue)
            {
                XsltListViewWebPartDefinition model = wpModel as XsltListViewWebPartDefinition;

                var context = listItemModelHost.HostWeb.Context;

                var list = LookupList(listItemModelHost, model);
                var view = list.GetView(_originalViewId.Value);

                view.DefaultView = true;
                view.Update();

                context.Load(view);
                context.ExecuteQuery();

                _originalViewId = null;
            }
        }

        private List LookupList(ListItemModelHost listItemModelHost, XsltListViewWebPartDefinition wpModel)
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
