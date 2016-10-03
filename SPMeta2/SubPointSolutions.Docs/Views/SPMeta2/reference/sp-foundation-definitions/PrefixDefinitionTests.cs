using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]

    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebApplication)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebApplicationModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class PrefixDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.PrefixDefinition")]

        [SampleMetadata(Title = "Add prefix",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimplePrefixDefinition()
        {
            var prefixDef = new PrefixDefinition
            {
                Path = "projects",
                PrefixType = BuiltInPrefixTypes.WildcardInclusion
            };

            var model = SPMeta2Model.NewWebApplicationModel(webApp =>
            {
                webApp.AddPrefix(prefixDef);
            });

            DeployModel(model);
        }

        #endregion
    }
}