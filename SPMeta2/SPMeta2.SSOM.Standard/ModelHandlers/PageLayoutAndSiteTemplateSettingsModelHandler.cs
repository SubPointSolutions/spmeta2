using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers
{
    public class PageLayoutAndSiteTemplateSettingsModelHandler : SSOMModelHandlerBase
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
            var publishingWeb = PublishingWeb.GetPublishingWeb(web);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            ProcessWebTemplateSettings(publishingWeb, definition);
            ProcessPageLayoutSettings(publishingWeb, definition);
            ProcessNewPageDefaultSettings(publishingWeb, definition);
            ProcessConverBlankSpacesIntoHyphenSetting(publishingWeb, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            publishingWeb.Update();
        }

        private void ProcessConverBlankSpacesIntoHyphenSetting(PublishingWeb publishingWeb, PageLayoutAndSiteTemplateSettingsDefinition definition)
        {
            var web = publishingWeb.Web;

            var key = "__AllowSpacesInNewPageName";
            var value = definition.ConverBlankSpacesIntoHyphen.HasValue
                ? definition.ConverBlankSpacesIntoHyphen.ToString()
                : false.ToString();

            if (!web.AllProperties.ContainsKey(key))
                web.AllProperties.Add(key, value);
            else
                web.AllProperties[key] = value;
        }

        private static void ProcessNewPageDefaultSettings(PublishingWeb publishingWeb, PageLayoutAndSiteTemplateSettingsDefinition definition)
        {
            var web = publishingWeb.Web;

            if (definition.InheritDefaultPageLayout.HasValue && definition.InheritDefaultPageLayout.Value)
                publishingWeb.InheritDefaultPageLayout();
            else if (definition.UseDefinedDefaultPageLayout.HasValue && definition.UseDefinedDefaultPageLayout.Value)
            {
                var publishingSite = new PublishingSite(web.Site);
                var pageLayouts = publishingSite.PageLayouts;

                var selectedPageLayout = pageLayouts.FirstOrDefault(t => t.Name.ToUpper() == definition.DefinedDefaultPageLayout.ToUpper()); ;

                if (selectedPageLayout != null)
                {
                    publishingWeb.SetDefaultPageLayout(
                        selectedPageLayout,
                        definition.ResetAllSubsitesToInheritDefaultPageLayout.HasValue
                            ? definition.ResetAllSubsitesToInheritDefaultPageLayout.Value
                            : false);
                }
            }
        }

        private static void ProcessPageLayoutSettings(PublishingWeb publishingWeb, PageLayoutAndSiteTemplateSettingsDefinition definition)
        {
            var web = publishingWeb.Web;

            if (definition.InheritPageLayouts.HasValue && definition.InheritPageLayouts.Value)
                publishingWeb.InheritAvailablePageLayouts();
            else if (definition.UseAnyPageLayout.HasValue && definition.UseAnyPageLayout.Value)
            {
                publishingWeb.AllowAllPageLayouts(definition.ResetAllSubsitesToInheritPageLayouts.HasValue
                    ? definition.ResetAllSubsitesToInheritPageLayouts.Value
                    : false);
            }
            else if (definition.UseDefinedPageLayouts.HasValue && definition.UseDefinedPageLayouts.Value)
            {
                var publishingSite = new PublishingSite(web.Site);
                var pageLayouts = publishingSite.PageLayouts;

                var selectedPageLayouts = new List<PageLayout>();

                foreach (var selectedLayoutName in definition.DefinedPageLayouts)
                {
                    var targetLayout = pageLayouts.FirstOrDefault(t => t.Name.ToUpper() == selectedLayoutName.ToUpper());

                    if (targetLayout != null)
                        selectedPageLayouts.Add(targetLayout);
                }

                if (selectedPageLayouts.Any())
                {
                    publishingWeb.SetAvailablePageLayouts(selectedPageLayouts.ToArray(),
                        definition.ResetAllSubsitesToInheritPageLayouts.HasValue
                            ? definition.ResetAllSubsitesToInheritPageLayouts.Value
                            : false);
                }
            }
        }

        private static void ProcessWebTemplateSettings(PublishingWeb publishingWeb, PageLayoutAndSiteTemplateSettingsDefinition definition)
        {
            var web = publishingWeb.Web;

            if (definition.InheritWebTemplates.HasValue && definition.InheritWebTemplates.Value)
                publishingWeb.InheritAvailableWebTemplates();
            else if (definition.UseAnyWebTemplate.HasValue && definition.UseAnyWebTemplate.Value)
            {
                publishingWeb.AllowAllWebTemplates(definition.ResetAllSubsitesToInheritWebTemplates.HasValue
                    ? definition.ResetAllSubsitesToInheritWebTemplates.Value
                    : false);
            }
            else if (definition.UseDefinedWebTemplates.HasValue && definition.UseDefinedWebTemplates.Value)
            {
                var currentLocaleId = (uint)web.CurrencyLocaleID;
                var webTemplates = new List<SPWebTemplate>();

                webTemplates.AddRange(web.Site.GetWebTemplates(currentLocaleId).OfType<SPWebTemplate>());
                webTemplates.AddRange(web.Site.GetCustomWebTemplates(currentLocaleId).OfType<SPWebTemplate>());

                var selectedWebTemplates = new Collection<SPWebTemplate>();

                foreach (var selectedWebTemplateName in definition.DefinedWebTemplates)
                {
                    var targetWebTemplate =
                        webTemplates.FirstOrDefault(t => t.Name.ToUpper() == selectedWebTemplateName.ToUpper());

                    if (targetWebTemplate != null)
                        selectedWebTemplates.Add(targetWebTemplate);
                }

                if (selectedWebTemplates.Any())
                {
                    publishingWeb.SetAvailableWebTemplates(selectedWebTemplates, 0,
                        definition.ResetAllSubsitesToInheritWebTemplates.HasValue
                            ? definition.ResetAllSubsitesToInheritWebTemplates.Value
                            : false);
                }
            }
        }

        #endregion
    }
}
