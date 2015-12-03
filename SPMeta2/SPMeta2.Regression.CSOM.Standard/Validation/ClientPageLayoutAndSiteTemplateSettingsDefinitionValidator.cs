using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Regression.CSOM.Standard.Extensions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class ClientPageLayoutAndSiteTemplateSettingsDefinitionValidator : PageLayoutAndSiteTemplateSettingsModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PageLayoutAndSiteTemplateSettingsDefinition>("model", value => value.RequireNotNull());

            var context = webModelHost.HostWeb.Context;
            var spObject = webModelHost.HostWeb;

            var assert = ServiceFactory.AssertService
                            .NewAssert(definition, spObject)
                            .ShouldNotBeNull(spObject);

            var pageLayouts = this.LoadPageLayouts(webModelHost);

            // web templates
            assert.SkipProperty(m => m.InheritWebTemplates, "InheritWebTemplates is not supported yet. Skipping");
            assert.SkipProperty(m => m.UseAnyWebTemplate, "UseAnyWebTemplate is not supported yet. Skipping");
            assert.SkipProperty(m => m.UseDefinedWebTemplates, "UseDefinedWebTemplates is not supported yet. Skipping");
            assert.SkipProperty(m => m.DefinedWebTemplates, "DefinedWebTemplates is not supported yet. Skipping");

            assert.SkipProperty(m => m.ResetAllSubsitesToInheritWebTemplates, "ResetAllSubsitesToInheritWebTemplates is not supported yet. Skipping");

            #region web settings

            //if (definition.InheritWebTemplates.HasValue)
            //    assert.ShouldBeEqual(m => m.InheritWebTemplates, o => o.IsInheritingAvailableWebTemplates);
            //else
            //    assert.SkipProperty(m => m.InheritWebTemplates, "InheritWebTemplates is NULL. Skipping");

            //if (definition.UseAnyWebTemplate.HasValue)
            //    assert.ShouldBeEqual(m => m.UseAnyWebTemplate, o => o.IsAllowingAllWebTemplates);
            //else
            //    assert.SkipProperty(m => m.UseAnyWebTemplate, "UseAnyWebTemplate is NULL. Skipping");

            //// TODO
            //if (definition.UseDefinedWebTemplates.HasValue)
            //{
            //    assert.SkipProperty(m => m.UseDefinedWebTemplates, "UseDefinedWebTemplates is not NULL. Validating DefinedWebTemplates.");

            //    assert.ShouldBeEqual((p, s, d) =>
            //    {
            //        var srcProp = s.GetExpressionValue(m => m.DefinedWebTemplates);
            //        var isValid = true;

            //        var currentTemplates = spObject.GetAvailableWebTemplates((uint)spObject.Web.CurrencyLocaleID);
            //        var definedTemplates = definition.DefinedWebTemplates;

            //        foreach (var defTemplate in definedTemplates)
            //        {
            //            if (!currentTemplates.OfType<SPWebTemplate>().Any(t => t.Name.ToUpper() == defTemplate.ToUpper()))
            //                isValid = false;
            //        }

            //        return new PropertyValidationResult
            //        {
            //            Tag = p.Tag,
            //            Src = srcProp,
            //            Dst = null,
            //            IsValid = isValid
            //        };
            //    });

            //}
            //else
            //{
            //    assert.SkipProperty(m => m.UseDefinedWebTemplates, "UseDefinedWebTemplates is NULL. Skipping");
            //    assert.SkipProperty(m => m.DefinedWebTemplates, "DefinedWebTemplates as UseDefinedWebTemplates is NULL. Skipping");
            //}

            //assert.SkipProperty(m => m.ResetAllSubsitesToInheritWebTemplates, "ResetAllSubsitesToInheritWebTemplates can't be validates. Skipping.");

            #endregion

            #region page layouts

            // layouts
            if (definition.InheritPageLayouts.HasValue)
                assert.ShouldBeEqual(m => m.InheritPageLayouts, o => o.GetInheritingAvailablePageLayouts());
            else
                assert.SkipProperty(m => m.InheritPageLayouts, "InheritPageLayouts is NULL. Skipping");

            if (definition.UseAnyPageLayout.HasValue)
                assert.ShouldBeEqual(m => m.UseAnyPageLayout, o => o.GetIsAllowingAllPageLayouts());
            else
                assert.SkipProperty(m => m.UseAnyPageLayout, "UseAnyPageLayout is NULL. Skipping");

            if (definition.UseDefinedPageLayouts.HasValue)
            {
                assert.SkipProperty(m => m.UseDefinedPageLayouts, "UseDefinedPageLayouts is not NULL. Validating DefinedPageLayouts.");

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.DefinedPageLayouts);
                    var isValid = true;

                    var currentTemplates = spObject.GetAvailablePageLayoutNames(pageLayouts);
                    var definedTemplates = definition.DefinedPageLayouts;

                    // URL check, should be root web relative
                    // https://github.com/SubPointSolutions/spmeta2/pull/740
                    // quick check
                    isValid = currentTemplates.All(t => t.Url.ToLower().StartsWith("_catalogs"));

                    foreach (var defTemplate in definedTemplates)
                    {
                        if (!currentTemplates.Any(t => t.Name.ToUpper() == defTemplate.ToUpper()))
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

            assert.SkipProperty(m => m.ResetAllSubsitesToInheritPageLayouts, "ResetAllSubsitesToInheritPageLayouts is not supported yet. Skipping");


            #endregion

            #region default page layout

            // default page layout
            if (definition.InheritDefaultPageLayout.HasValue)
                assert.ShouldBeEqual(m => m.InheritDefaultPageLayout, o => o.GetIsInheritingAvailablePageLayouts());
            else
                assert.SkipProperty(m => m.InheritDefaultPageLayout, "InheritDefaultPageLayout is NULL. Skipping");

            if (definition.UseDefinedDefaultPageLayout.HasValue)
            {
                assert.SkipProperty(m => m.UseDefinedDefaultPageLayout, "UseDefinedDefaultPageLayout is not NULL. Validating DefinedDefaultPageLayout.");

                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.DefinedDefaultPageLayout);
                    var isValid = true;

                    var currentPageLayout = spObject.GetDefaultPageLayoutName(pageLayouts);

                    // URL check, should be root web relative
                    // https://github.com/SubPointSolutions/spmeta2/pull/740
                    // quick check
                    isValid = currentPageLayout.Url.ToLower().StartsWith("_catalogs");

                    if (isValid)
                    {
                        isValid = currentPageLayout.Name.ToUpper() == definition.DefinedDefaultPageLayout.ToUpper();
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
                assert.SkipProperty(m => m.UseDefinedDefaultPageLayout, "UseDefinedDefaultPageLayout is NULL. Skipping");
                assert.SkipProperty(m => m.DefinedDefaultPageLayout, "DefinedDefaultPageLayout as UseDefinedDefaultPageLayout is NULL. Skipping");
            }

            assert.SkipProperty(m => m.ResetAllSubsitesToInheritDefaultPageLayout, "ResetAllSubsitesToInheritDefaultPageLayout can't be validates. Skipping.");

            #endregion

            #region space settings

            // space settings
            if (definition.ConverBlankSpacesIntoHyphen.HasValue)
                assert.ShouldBeEqual(m => m.ConverBlankSpacesIntoHyphen, o => o.GetConverBlankSpacesIntoHyphen());
            else
                assert.SkipProperty(m => m.ConverBlankSpacesIntoHyphen, "ConverBlankSpacesIntoHyphen is NULL. Skipping");

            #endregion
        }

        #endregion
    }

}
