using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHandlers;
using System.Xml;

namespace SPMeta2.CSOM.Standard.ModelHandlers
{
    public class PageLayoutAndSiteTemplateSettingsModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PageLayoutAndSiteTemplateSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PageLayoutAndSiteTemplateSettingsDefinition>("model", value => value.RequireNotNull());

            DeployDefinition(modelHost, webModelHost, definition);
        }

        private void DeployDefinition(object modelHost, WebModelHost webModelHost, PageLayoutAndSiteTemplateSettingsDefinition definition)
        {
            var web = webModelHost.HostWeb;
            var context = web.Context;

            var publishingWeb = PublishingWeb.GetPublishingWeb(context, web);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            var pageLayouts = LoadPageLayouts(webModelHost);

            ProcessWebTemplateSettings(webModelHost, publishingWeb, definition, pageLayouts);
            ProcessPageLayoutSettings(webModelHost, publishingWeb, definition, pageLayouts);
            ProcessNewPageDefaultSettings(webModelHost, publishingWeb, definition, pageLayouts);
            ProcessConverBlankSpacesIntoHyphenSetting(webModelHost, publishingWeb, definition, pageLayouts);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            web.Update();
            context.ExecuteQuery();
        }

        private void ProcessConverBlankSpacesIntoHyphenSetting(
            WebModelHost webModelHost,
            PublishingWeb publishingWeb,
            PageLayoutAndSiteTemplateSettingsDefinition definition,
            List<ListItem> pageLayouts)
        {
            var web = publishingWeb.Web;

            var key = "__AllowSpacesInNewPageName";
            var value = definition.ConverBlankSpacesIntoHyphen.HasValue
                ? definition.ConverBlankSpacesIntoHyphen.ToString()
                : false.ToString();

            SetPropertyBagValue(web, key, value);
        }

        public static void SetPropertyBagValue(Web web, string key, string value)
        {
            var context = web.Context;

            if (!web.IsPropertyAvailable("AllProperties"))
            {
                context.Load(web);
                context.Load(web, w => w.AllProperties);
                context.ExecuteQuery();
            }

            if (!web.AllProperties.FieldValues.ContainsKey(key))
                web.AllProperties.FieldValues.Add(key, value);
            else
                web.AllProperties[key] = value;

            web.Update();
            context.ExecuteQuery();
        }

        public static object GetPropertyBagValue(Web web, string key)
        {
            var context = web.Context;

            if (!web.IsPropertyAvailable("AllProperties"))
            {
                context.Load(web);
                context.Load(web, w => w.AllProperties);
                context.ExecuteQuery();
            }

            if (!web.AllProperties.FieldValues.ContainsKey(key))
                return null;
            else
                return web.AllProperties[key];
        }


        private static void ProcessNewPageDefaultSettings(
            WebModelHost webModelHost,
            PublishingWeb publishingWeb,
            PageLayoutAndSiteTemplateSettingsDefinition definition,
            List<ListItem> pageLayouts)
        {
            var web = publishingWeb.Web;
            var context = web.Context;

            if (definition.InheritDefaultPageLayout.HasValue && definition.InheritDefaultPageLayout.Value)
            {
                SetPropertyBagValue(web, "__DefaultPageLayout", "__inherit");
            }
            else if (definition.UseDefinedDefaultPageLayout.HasValue && definition.UseDefinedDefaultPageLayout.Value)
            {
                var selectedLayoutName = definition.DefinedDefaultPageLayout;
                var targetLayout = pageLayouts.FirstOrDefault(t => t["FileLeafRef"].ToString().ToUpper() == selectedLayoutName.ToUpper());

                if (targetLayout != null)
                {
                    var resultString = CreateLayoutXmlString(targetLayout);
                    SetPropertyBagValue(web, "__DefaultPageLayout", resultString);
                }
            }
        }

        protected List<ListItem> LoadPageLayouts(WebModelHost webModelHost)
        {
            var rootWeb = webModelHost.HostSite.RootWeb;
            var context = rootWeb.Context;

            var masterPageList = rootWeb.QueryAndGetListByUrl("/_catalogs/masterpage");

            var pageLayouts = masterPageList.GetItems(CamlQueryTemplates.ItemsByFieldValueBeginsWithQuery("ContentTypeId", BuiltInPublishingContentTypeId.PageLayout));
            context.Load(pageLayouts);
            context.ExecuteQuery();

            return pageLayouts.ToList();
        }

        private static void ProcessPageLayoutSettings(
            WebModelHost webModelHost,
            PublishingWeb publishingWeb,
            PageLayoutAndSiteTemplateSettingsDefinition definition,
            List<ListItem> pageLayouts)
        {
            var rootWeb = webModelHost.HostSite.RootWeb;
            var web = publishingWeb.Web;
            var context = web.Context;

            if (definition.InheritPageLayouts.HasValue && definition.InheritPageLayouts.Value)
            {
                SetPropertyBagValue(web, "__PageLayouts", "__inherit");
            }
            else if (definition.UseAnyPageLayout.HasValue && definition.UseAnyPageLayout.Value)
            {
                SetPropertyBagValue(web, "__PageLayouts", "");
            }
            else if (definition.UseDefinedPageLayouts.HasValue && definition.UseDefinedPageLayouts.Value)
            {
                var selectedPageLayouts = new List<ListItem>();

                foreach (var selectedLayoutName in definition.DefinedPageLayouts)
                {
                    var targetLayout = pageLayouts.FirstOrDefault(t => t["FileLeafRef"].ToString().ToUpper() == selectedLayoutName.ToUpper());

                    if (targetLayout != null)
                        selectedPageLayouts.Add(targetLayout);
                }

                if (selectedPageLayouts.Any())
                {
                    var resultString = CreateLayoutsXmlString(selectedPageLayouts);
                    SetPropertyBagValue(web, "__PageLayouts", resultString);
                }
            }
        }

        private static string CreateLayoutXmlString(ListItem pageLayout)
        {
            var xmlDocument = new XmlDocument();
            var rootXmlNode = xmlDocument.CreateElement("pagelayouts");
            xmlDocument.AppendChild(rootXmlNode);

            var xmlNode = xmlDocument.CreateElement("layout");

            var xmlAttribute = xmlDocument.CreateAttribute("guid");
            xmlAttribute.Value = pageLayout[BuiltInInternalFieldNames.UniqueId].ToString();

            var xmlAttribute2 = xmlDocument.CreateAttribute("url");
            xmlAttribute2.Value = pageLayout[BuiltInInternalFieldNames.FileRef].ToString();

            xmlNode.Attributes.SetNamedItem(xmlAttribute);
            xmlNode.Attributes.SetNamedItem(xmlAttribute2);

            return xmlNode.OuterXml;
        }

        private static string CreateLayoutsXmlString(IEnumerable<ListItem> pageLayouts)
        {
            var xmlDocument = new XmlDocument();
            var rootXmlNode = xmlDocument.CreateElement("pagelayouts");
            xmlDocument.AppendChild(rootXmlNode);

            foreach (var pageLayout in pageLayouts)
            {
                var xmlNode = xmlDocument.CreateElement("layout");

                var xmlAttribute = xmlDocument.CreateAttribute("guid");
                xmlAttribute.Value = pageLayout[BuiltInInternalFieldNames.UniqueId].ToString();

                var xmlAttribute2 = xmlDocument.CreateAttribute("url");
                xmlAttribute2.Value = pageLayout[BuiltInInternalFieldNames.FileRef].ToString();

                xmlNode.Attributes.SetNamedItem(xmlAttribute);
                xmlNode.Attributes.SetNamedItem(xmlAttribute2);

                rootXmlNode.AppendChild(xmlNode);
            }

            return rootXmlNode.OuterXml;
        }

        private static void ProcessWebTemplateSettings(
            WebModelHost webModelHost,
            PublishingWeb publishingWeb,
            PageLayoutAndSiteTemplateSettingsDefinition definition,
            List<ListItem> pageLayouts)
        {
            //var web = publishingWeb.Web;

            //if (definition.InheritWebTemplates.HasValue && definition.InheritWebTemplates.Value)
            //    publishingWeb.InheritAvailableWebTemplates();
            //else if (definition.UseAnyWebTemplate.HasValue && definition.UseAnyWebTemplate.Value)
            //{
            //    publishingWeb.AllowAllWebTemplates(definition.ResetAllSubsitesToInheritWebTemplates.HasValue
            //        ? definition.ResetAllSubsitesToInheritWebTemplates.Value
            //        : false);
            //}
            //else if (definition.UseDefinedWebTemplates.HasValue && definition.UseDefinedWebTemplates.Value)
            //{
            //    var currentLocaleId = (uint)web.CurrencyLocaleID;
            //    var webTemplates = new List<SPWebTemplate>();

            //    webTemplates.AddRange(web.Site.GetWebTemplates(currentLocaleId).OfType<SPWebTemplate>());
            //    webTemplates.AddRange(web.Site.GetCustomWebTemplates(currentLocaleId).OfType<SPWebTemplate>());

            //    var selectedWebTemplates = new Collection<SPWebTemplate>();

            //    foreach (var selectedWebTemplateName in definition.DefinedWebTemplates)
            //    {
            //        var targetWebTemplate =
            //            webTemplates.FirstOrDefault(t => t.Name.ToUpper() == selectedWebTemplateName.ToUpper());

            //        if (targetWebTemplate != null)
            //            selectedWebTemplates.Add(targetWebTemplate);
            //    }

            //    if (selectedWebTemplates.Any())
            //    {
            //        publishingWeb.SetAvailableWebTemplates(selectedWebTemplates, 0,
            //            definition.ResetAllSubsitesToInheritWebTemplates.HasValue
            //                ? definition.ResetAllSubsitesToInheritWebTemplates.Value
            //                : false);
            //    }
            //}
        }

        #endregion
    }
}
