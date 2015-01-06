using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class PageLayoutAndSiteTemplateSettingsDefinitionValidator : PageLayoutAndSiteTemplateSettingsModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PageLayoutAndSiteTemplateSettingsDefinition>("model", value => value.RequireNotNull());

            var spObject = PublishingWeb.GetPublishingWeb(webModelHost.HostWeb);

            var assert = ServiceFactory.AssertService
                            .NewAssert(definition, spObject)
                            .ShouldNotBeNull(spObject);

            // web templates
            if (definition.InheritWebTemplates.HasValue)
                assert.ShouldBeEqual(m => m.InheritWebTemplates, o => o.IsInheritingAvailableWebTemplates);
            else
                assert.SkipProperty(m => m.InheritWebTemplates, "InheritWebTemplates is NULL. Skipping");

            if (definition.UseAnyWebTemplate.HasValue)
                assert.ShouldBeEqual(m => m.UseAnyWebTemplate, o => o.IsAllowingAllWebTemplates);
            else
                assert.SkipProperty(m => m.UseAnyWebTemplate, "UseAnyWebTemplate is NULL. Skipping");

            // TODO
            if (definition.UseDefinedWebTemplates.HasValue)
            {
                assert.SkipProperty(m => m.UseDefinedWebTemplates, "UseDefinedWebTemplates is not NULL. Validating DefinedWebTemplates.");

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.DefinedWebTemplates);
                    var isValid = true;

                    var currentTemplates = spObject.GetAvailableWebTemplates((uint)spObject.Web.CurrencyLocaleID);
                    var definedTemplates = definition.DefinedWebTemplates;

                    foreach (var defTemplate in definedTemplates)
                    {
                        if (!currentTemplates.OfType<SPWebTemplate>().Any(t => t.Name.ToUpper() == defTemplate.ToUpper()))
                            isValid = false;
                    }

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

            }
            else
            {
                assert.SkipProperty(m => m.UseDefinedWebTemplates, "UseDefinedWebTemplates is NULL. Skipping");
                assert.SkipProperty(m => m.DefinedWebTemplates, "DefinedWebTemplates as UseDefinedWebTemplates is NULL. Skipping");
            }

            assert.SkipProperty(m => m.ResetAllSubsitesToInheritWebTemplates, "ResetAllSubsitesToInheritWebTemplates can't be validates. Skipping.");

            // layouts
            if (definition.InheritPageLayouts.HasValue)
                assert.ShouldBeEqual(m => m.InheritPageLayouts, o => o.IsInheritingAvailablePageLayouts);
            else
                assert.SkipProperty(m => m.InheritPageLayouts, "InheritPageLayouts is NULL. Skipping");

            if (definition.UseAnyPageLayout.HasValue)
                assert.ShouldBeEqual(m => m.UseAnyPageLayout, o => o.IsAllowingAllPageLayouts);
            else
                assert.SkipProperty(m => m.UseAnyPageLayout, "UseAnyPageLayout is NULL. Skipping");

            assert.SkipProperty(m => m.ResetAllSubsitesToInheritPageLayouts, "ResetAllSubsitesToInheritPageLayouts can't be validates. Skipping.");

            if (definition.UseDefinedPageLayouts.HasValue)
            {
                assert.SkipProperty(m => m.UseDefinedPageLayouts, "UseDefinedPageLayouts is not NULL. Validating DefinedPageLayouts.");

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.DefinedPageLayouts);
                    var isValid = true;

                    var currentTemplates = spObject.GetAvailablePageLayouts();
                    var definedTemplates = definition.DefinedPageLayouts;

                    foreach (var defTemplate in definedTemplates)
                    {
                        if (!currentTemplates.OfType<PageLayout>().Any(t => t.Name.ToUpper() == defTemplate.ToUpper()))
                            isValid = false;
                    }

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

            }
            else
            {
                assert.SkipProperty(m => m.UseDefinedPageLayouts, "UseDefinedPageLayouts is NULL. Skipping");
                assert.SkipProperty(m => m.DefinedPageLayouts, "DefinedPageLayouts as UseDefinedPageLayouts is NULL. Skipping");
            }

            // default page layout
            if (definition.InheritDefaultPageLayout.HasValue)
                assert.ShouldBeEqual(m => m.InheritDefaultPageLayout, o => o.IsInheritingAvailablePageLayouts);
            else
                assert.SkipProperty(m => m.InheritDefaultPageLayout, "InheritDefaultPageLayout is NULL. Skipping");

            if (definition.UseDefinedDefaultPageLayout.HasValue)
            {
                assert.SkipProperty(m => m.UseDefinedDefaultPageLayout, "UseDefinedDefaultPageLayout is not NULL. Validating DefinedDefaultPageLayout.");

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.DefinedDefaultPageLayout);
                    var isValid = true;

                    var currentPageLayout = spObject.DefaultPageLayout;

                    isValid = currentPageLayout.Name.ToUpper() == definition.DefinedDefaultPageLayout.ToUpper();

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });

            }
            else
            {
                assert.SkipProperty(m => m.UseDefinedDefaultPageLayout, "UseDefinedDefaultPageLayout is NULL. Skipping");
                assert.SkipProperty(m => m.DefinedDefaultPageLayout, "DefinedDefaultPageLayout as UseDefinedDefaultPageLayout is NULL. Skipping");
            }

            assert.SkipProperty(m => m.ResetAllSubsitesToInheritDefaultPageLayout, "ResetAllSubsitesToInheritDefaultPageLayout can't be validates. Skipping.");

            // space settings
            if (definition.ConverBlankSpacesIntoHyphen.HasValue)
                assert.ShouldBeEqual(m => m.ConverBlankSpacesIntoHyphen, o => o.GetConverBlankSpacesIntoHyphen());
            else
                assert.SkipProperty(m => m.ConverBlankSpacesIntoHyphen, "ConverBlankSpacesIntoHyphen is NULL. Skipping");
        }

        #endregion
    }

    public static class PublishingWebExtensions
    {


        public static bool GetInheritWebTemplates(this PublishingWeb publishingWeb)
        {
            return publishingWeb.IsInheritingAvailableWebTemplates;
        }

        public static bool GetConverBlankSpacesIntoHyphen(this PublishingWeb publishingWeb)
        {
            var web = publishingWeb.Web;

            var key = "__AllowSpacesInNewPageName";

            if (!web.AllProperties.ContainsKey(key))
                return false;

            return ConvertUtils.ToBool(web.AllProperties[key]).Value;
        }
    }
}
