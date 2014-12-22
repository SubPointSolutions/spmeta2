using System.Linq;
using SPMeta2.Containers.Assertion;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.SSOM.Standard.ModelHandlers;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation
{
    public class SearchConfigurationDefinitionValidator : SearchConfigurationModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SearchConfigurationDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentSearchConfiguration(siteModelHost.HostSite);
            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.SearchConfiguration);

                var srcNode = SearchTemplatesUtils.GetSetSourceNode(s.SearchConfiguration);
                var dstNodes = SearchTemplatesUtils.GetSetSourceNodes(d);

                var dstNode = dstNodes.FirstOrDefault(
                                    n => SearchTemplatesUtils.GetSetSourceName(n) ==
                                        SearchTemplatesUtils.GetSetSourceName(srcNode));

                var isValid = dstNode != null &&
                    SearchTemplatesUtils.GetSetSourceDescription(dstNode) == SearchTemplatesUtils.GetSetSourceDescription(srcNode);

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
}
