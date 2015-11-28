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
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHandlers.Fields;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.SSOM.ModelHandlers.Webparts
{
    public class XsltListViewWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(XsltListViewWebPartDefinition); }
        }

        #endregion

        #region methods

        private WebpartPageModelHost _host;

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            _host = host;

            var typedModel = webpartModel.WithAssertAndCast<XsltListViewWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(XsltListViewWebPart).AssemblyQualifiedName;
        }

        protected override void OnAfterDeployModel(WebpartPageModelHost host, WebPartDefinition definition)
        {
            var typedDefinition = definition.WithAssertAndCast<XsltListViewWebPartDefinition>("webpartModel", value => value.RequireNotNull());

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
                    var xsltWebPart = existingWebPart as XsltListViewWebPart;

                    if (xsltWebPart != null)
                    {

                        // big TODO for .NET 35
                        // xsltWebPart.View does not exist for .NET 35
                        // the implementation will be done upon the community demand

#if !NET35

                        var targetView = xsltWebPart.View;

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

#endif
                    }
                }
            }
        }


        public static SPList GetTargetList(SPWeb targetWeb, string listTitle, string listUrl, Guid? listId)
        {
            SPList result = null;

            if (listId.HasValue && listId != default(Guid))
                result = targetWeb.Lists[listId.Value];
            else if (!string.IsNullOrEmpty(listUrl))
                result = targetWeb.GetList(SPUrlUtility.CombineUrl(targetWeb.ServerRelativeUrl, listUrl));
            else if (!string.IsNullOrEmpty(listTitle))
                result = targetWeb.Lists.TryGetList(listTitle);
            else
            {
                throw new SPMeta2Exception("ListUrl, ListTitle or ListId should be defined.");
            }

            return result;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<XsltListViewWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedModel = webpartModel.WithAssertAndCast<XsltListViewWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            var web = _host.SPLimitedWebPartManager.Web;

            // bind list
            var targetWeb = web;

            if (!string.IsNullOrEmpty(typedModel.WebUrl) || typedModel.WebId.HasGuidValue())
                targetWeb = new LookupFieldModelHandler().GetTargetWeb(web.Site, typedModel.WebUrl, typedModel.WebId);

            var list = GetTargetList(targetWeb, typedModel.ListTitle, typedModel.ListUrl, typedModel.ListId);

            if (list != null)
            {
                typedWebpart.ListName = list.ID.ToString("B").ToUpperInvariant();
                typedWebpart.TitleUrl = list.DefaultViewUrl;
            }

            // view check
            if (list != null)
            {
                SPView srcView = null;

                if (typedModel.ViewId.HasValue && typedModel.ViewId != default(Guid))
                    srcView = list.Views[typedModel.ViewId.Value];
                else if (!string.IsNullOrEmpty(typedModel.ViewName))
                    srcView = list.Views[typedModel.ViewName];

                if (srcView != null)
                {
                    if (!string.IsNullOrEmpty(typedWebpart.ViewGuid))
                    {
                        // update hidden view, otherwise we can have weird SharePoint exception
                        // https://github.com/SubPointSolutions/spmeta2/issues/487

                        var hiddenView = list.Views[new Guid(typedWebpart.ViewGuid)];

                        hiddenView.SetViewXml(srcView.GetViewXml());
                     
                        hiddenView.Update();
                    }
                    else
                    {
                        typedWebpart.ViewGuid = srcView.ID.ToString("B").ToUpperInvariant();
                    }

                    typedWebpart.TitleUrl = srcView.ServerRelativeUrl;
                }
            }

            // able to 'reset', if NULL or use list-view based URLs
            if (!string.IsNullOrEmpty(typedModel.TitleUrl))
                typedWebpart.TitleUrl = typedModel.TitleUrl;

            // weird, but it must be set to avoid null-ref exceptions
            typedWebpart.GhostedXslLink = "main.xsl";

#if !NET35
            // rest
            typedWebpart.JSLink = typedModel.JSLink;
#endif

            if (typedModel.CacheXslStorage.HasValue)
                typedWebpart.CacheXslStorage = typedModel.CacheXslStorage.Value;

            if (typedModel.CacheXslTimeOut.HasValue)
                typedWebpart.CacheXslTimeOut = typedModel.CacheXslTimeOut.Value;

#if !NET35
            if (typedModel.ShowTimelineIfAvailable.HasValue)
                typedWebpart.ShowTimelineIfAvailable = typedModel.ShowTimelineIfAvailable.Value;
#endif

            if (!string.IsNullOrEmpty(typedModel.Xsl))
            {
                typedWebpart.Xsl = typedModel.Xsl;
            }

            if (!string.IsNullOrEmpty(typedModel.XslLink))
            {
                var urlValue = typedModel.XslLink;

                typedWebpart.XslLink = urlValue;
            }

            if (!string.IsNullOrEmpty(typedModel.XmlDefinition))
            {
                typedWebpart.XmlDefinition = typedModel.XmlDefinition;
            }

            if (!string.IsNullOrEmpty(typedModel.XmlDefinitionLink))
            {
                var urlValue = typedModel.XmlDefinitionLink;
                typedWebpart.XmlDefinitionLink = urlValue;
            }

            if (!string.IsNullOrEmpty(typedModel.GhostedXslLink))
            {
                var urlValue = typedModel.GhostedXslLink;
                typedWebpart.GhostedXslLink = urlValue;
            }

            if (!string.IsNullOrEmpty(typedModel.BaseXsltHashKey))
                typedWebpart.BaseXsltHashKey = typedModel.BaseXsltHashKey;

            if (typedModel.DisableColumnFiltering.HasValue)
                typedWebpart.DisableColumnFiltering = typedModel.DisableColumnFiltering.Value;

#if !NET35
            if (typedModel.DisableSaveAsNewViewButton.HasValue)
                typedWebpart.DisableSaveAsNewViewButton = typedModel.DisableSaveAsNewViewButton.Value;

            if (typedModel.DisableViewSelectorMenu.HasValue)
                typedWebpart.DisableViewSelectorMenu = typedModel.DisableViewSelectorMenu.Value;

            if (typedModel.InplaceSearchEnabled.HasValue)
                typedWebpart.InplaceSearchEnabled = typedModel.InplaceSearchEnabled.Value;
#endif
        }

        #endregion
    }
}
