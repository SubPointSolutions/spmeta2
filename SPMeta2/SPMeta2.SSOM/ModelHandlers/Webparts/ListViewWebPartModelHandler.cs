using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.SSOM.ModelHandlers.Webparts
{
    public class ListViewWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        private WebpartPageModelHost _host;

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            _host = host;

            var typedModel = webpartModel.WithAssertAndCast<ListViewWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(ListViewWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<ListViewWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedModel = webpartModel.WithAssertAndCast<ListViewWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            var web = _host.SPLimitedWebPartManager.Web;

            var targetWeb = web;

            if (!string.IsNullOrEmpty(typedModel.WebUrl) || typedModel.WebId.HasGuidValue())
                targetWeb = new LookupFieldModelHandler().GetTargetWeb(web.Site, typedModel.WebUrl, typedModel.WebId);

            var list = XsltListViewWebPartModelHandler.GetTargetList(targetWeb, typedModel.ListTitle, typedModel.ListUrl, typedModel.ListId);

            typedWebpart.ListName = list.ID.ToString();
            typedWebpart.ListId = list.ID;

            // view check
            if (list != null)
            {
                SPView view = null;

                if (typedModel.ViewId.HasGuidValue())
                    view = list.Views[typedModel.ViewId.Value];
                else if (!string.IsNullOrEmpty(typedModel.ViewName))
                    view = list.Views[typedModel.ViewName];
                else if (!string.IsNullOrEmpty(typedModel.ViewUrl))
                {
                    view = list.Views.OfType<SPView>()
                        .FirstOrDefault(v => v.ServerRelativeUrl.ToUpper().EndsWith(typedModel.ViewUrl.ToUpper()));
                }

                if (view != null)
                {
                    typedWebpart.ViewGuid = view.ID.ToString("B").ToUpperInvariant();
                    typedWebpart.TitleUrl = view.ServerRelativeUrl;
                }
            }

            // able to 'reset', if NULL or use list-view based URLs
            if (!string.IsNullOrEmpty(typedModel.TitleUrl))
                typedWebpart.TitleUrl = typedModel.TitleUrl;
        }

        protected override void OnAfterDeployModel(WebpartPageModelHost host, WebPartDefinition definition)
        {
            var typedDefinition = definition.WithAssertAndCast<ListViewWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(typedDefinition.Toolbar))
            {
                var existingWebPart = host.SPLimitedWebPartManager
                    .WebParts
                    .OfType<System.Web.UI.WebControls.WebParts.WebPart>()
                    .FirstOrDefault(wp => !string.IsNullOrEmpty(wp.ID) &&
                                          wp.ID.ToUpper() == definition.Id.ToUpper());

                if (existingWebPart != null)
                {
                    // patching up the view -> ToolbarType
                    var xsltWebPart = existingWebPart as ListViewWebPart;

                    if (xsltWebPart != null)
                    {
                        if (!string.IsNullOrEmpty(xsltWebPart.ViewGuid))
                        {
                            var targetWeb = new LookupFieldModelHandler().GetTargetWeb(
                                            host.PageListItem.Web.Site,
                                            typedDefinition.WebUrl,
                                            typedDefinition.WebId);

                            var list = XsltListViewWebPartModelHandler.GetTargetList(targetWeb,
                                            typedDefinition.ListTitle,
                                            typedDefinition.ListUrl,
                                            typedDefinition.ListId);

                            var targetView = list.Views[ConvertUtils.ToGuid(xsltWebPart.ViewGuid).Value];

                            // fixing up the Toolbar
                            if (!string.IsNullOrEmpty(typedDefinition.Toolbar))
                            {
                                var htmlSchemaXml = XDocument.Parse(targetView.HtmlSchemaXml);

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

                                var field = targetView.GetType()
                                    .GetProperty("ListViewXml",
                                        BindingFlags.NonPublic | BindingFlags.Instance);

                                if (field != null)
                                {
                                    field.SetValue(targetView, htmlSchemaXml.Root.GetInnerXmlAsString(), null);
                                }
                            }

                            targetView.Update();
                        }
                    }
                }
            }
        }

        #endregion
    }
}
