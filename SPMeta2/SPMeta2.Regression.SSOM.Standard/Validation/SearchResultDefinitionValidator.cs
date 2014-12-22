using System;
using System.Linq;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.Search.Administration.Query;
using Microsoft.SharePoint.Publishing;
using SPMeta2.Common;
using SPMeta2.Containers.Assertion;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation
{
    public class SearchResultDefinitionValidator : SearchResultModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SearchResultDefinition>("model", value => value.RequireNotNull());

            FederationManager federationManager = null;
            SearchObjectOwner searchOwner = null;

            var spObject = GetCurrentSource(siteModelHost.HostSite, definition, out federationManager, out searchOwner);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                                 .ShouldBeEqual(m => m.Name, o => o.Name)
                                 .ShouldBeEqual(m => m.Query, o => o.GetQuery())
                                 .ShouldBeEqual(m => m.Description, o => o.Description);

            if (definition.ProviderId.HasValue)
                assert.ShouldBeEqual(m => m.ProviderId, o => o.ProviderId);
            else
                assert.SkipProperty(m => m.ProviderId, "ProviderId is NULL. Skipping.");

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.ProviderName);
                var searchProvider = GetProviderByName(federationManager, s.ProviderName);

                var isValid = searchProvider.Name == s.ProviderName;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.IsDefault);
                var defaultSource = GetDefaultSource(federationManager, searchOwner);

                var isValid = defaultSource.Name == d.Name;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });
        }

        #endregion
    }

    internal static class SourceExtensions
    {
        public static string GetQuery(this Source source)
        {
            return source.QueryTransform.QueryTemplate;
        }
    }
}
