using System;
using System.IO;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.Extensions;
using SPMeta2.Utils;
using System.Web.UI.WebControls.WebParts;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebPartPageModelHandler : ModelHandlerBase
    {
        #region properties

        private const string WebPartPageCmdTemplate =
                                          "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                          "<Method ID=\"0, NewWebPage\">" +
                                          "<SetList Scope=\"Request\">{0}</SetList>" +
                                          "<SetVar Name=\"ID\">New</SetVar>" +
                                          "<SetVar Name=\"Title\">{1}</SetVar>" +
                                          "<SetVar Name=\"Cmd\">NewWebPage</SetVar>" +
                                          "<SetVar Name=\"WebPartPageTemplate\">{2}</SetVar>" +
                                          "<SetVar Name=\"Type\">WebPartPage</SetVar>" +
                                          "<SetVar Name=\"Overwrite\">{3}</SetVar>" +
                                          "</Method>";

        #endregion

        #region methods

        public override Type TargetType
        {
            get { return typeof(WebPartPageDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var list = modelHost.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var webpartPageModel = model.WithAssertAndCast<WebPartPageDefinition>("model", value => value.RequireNotNull());

            var targetPage = GetOrCreateNewWebPartPage(list, webpartPageModel);

            using (var webPartManager = targetPage.File.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                var webpartPageHost = new WebpartPageModelHost
                {
                    PageListItem = targetPage,
                    SPLimitedWebPartManager = webPartManager
                };

                action(webpartPageHost);
            }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var webpartPageModel = model.WithAssertAndCast<WebPartPageDefinition>("model", value => value.RequireNotNull());

            var targetPage = GetOrCreateNewWebPartPage(list, webpartPageModel);

            // gosh, it really does not have a title
            //targetPage[SPBuiltInFieldId.Title] = webpartPageModel.Title;

            targetPage.Update();
        }

        protected SPListItem FindWebPartPage(SPList list, WebPartPageDefinition webpartPageModel)
        {
            var webPartPageName = GetSafeWebPartPageFileName(webpartPageModel);

            if (!webPartPageName.EndsWith(".aspx"))
                webPartPageName += ".aspx";

            return list.Items.OfType<SPListItem>().FirstOrDefault(i => string.Compare(i.Name, webPartPageName, true) == 0);
        }

        protected string GetSafeWebPartPageFileName(WebPartPageDefinition webpartPageModel)
        {
            var webPartPageName = webpartPageModel.FileName;

            if (!webPartPageName.EndsWith(".aspx"))
                webPartPageName += ".aspx";

            return webPartPageName;
        }

        private SPListItem GetOrCreateNewWebPartPage(SPList list, WebPartPageDefinition webpartPageModel)
        {
            var targetPage = FindWebPartPage(list, webpartPageModel);

            if (targetPage == null || webpartPageModel.NeedOverride)
            {
                var webPartPageName = GetSafeWebPartPageFileName(webpartPageModel);

                // web part page name has to be without .aspx extensions in xmlCmd for ProcessBatchData method!
                var xmlCmd = string.Format(WebPartPageCmdTemplate,
                                   new object[]{
                                            list.ID.ToString(), 
                                            Path.GetFileNameWithoutExtension(webPartPageName),
                                            (int)webpartPageModel.PageLayoutTemplate,
                                            webpartPageModel.NeedOverride.ToString().ToLower()});

                var result = list.ParentWeb.ProcessBatchData(xmlCmd);

                // TODO. analyse the result
                // <Result ID="0, NewWebPage" Code="0"></Result>

                targetPage = FindWebPartPage(list, webpartPageModel);
            }

            return targetPage;
        }

        #endregion
    }
}
