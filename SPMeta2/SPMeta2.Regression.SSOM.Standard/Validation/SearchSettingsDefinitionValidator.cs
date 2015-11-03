using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Config;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class SearchSettingsDefinitionValidator : SearchSettingsModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<SearchSettingsDefinition>("model", value => value.RequireNotNull());

            SPWeb spObject = null;
            SearchSettingsConfig searchSettings = null;
            var searchCenterUrl = String.Empty;

            if (modelHost is SiteModelHost)
            {
                spObject = (modelHost as SiteModelHost).HostSite.RootWeb;

                searchSettings = GetCurrentSearchConfigAtSiteLevel(spObject);
                searchCenterUrl = GetSearchCenterUrlAtSiteLevel(spObject);
            }
            else if (modelHost is WebModelHost)
            {
                spObject = (modelHost as WebModelHost).HostWeb;

                searchSettings = GetCurrentSearchConfigAtWebLevel(spObject);
                searchCenterUrl = GetSearchCenterUrlAtWebLevel(spObject);
            }

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);

            if (!string.IsNullOrEmpty(definition.SearchCenterUrl))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.SearchCenterUrl);
                    var isValid = s.SearchCenterUrl == searchCenterUrl;

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
                assert.SkipProperty(m => m.SearchCenterUrl);
            }

            if (!string.IsNullOrEmpty(definition.UseCustomResultsPageUrl))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.UseCustomResultsPageUrl);
                    var isValid = s.UseCustomResultsPageUrl == searchSettings.ResultsPageAddress;

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
                assert.SkipProperty(m => m.UseCustomResultsPageUrl);
            }

            if (definition.UseParentResultsPageUrl.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.UseParentResultsPageUrl);
                    var isValid = s.UseParentResultsPageUrl.Value == searchSettings.Inherit;

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
                assert.SkipProperty(m => m.UseParentResultsPageUrl, "UseParentResultsPageUrl is null");
            }

            if (definition.UseFirstSearchNavigationNode.HasValue)
            {

            }
            else
            {
                assert.SkipProperty(m => m.UseFirstSearchNavigationNode, "UseFirstSearchNavigationNode is null");
            }
        }

        #endregion
    }
}
