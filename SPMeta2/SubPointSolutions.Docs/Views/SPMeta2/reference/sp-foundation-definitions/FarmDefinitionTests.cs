using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Enumerations;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.Farm)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.FarmModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class FarmDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.FarmDefinition")]

        [SampleMetadata(Title = "Add farm feature",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleFarmDefinition()
        {
            var farmFeature = BuiltInFarmFeatures.SiteMailboxes.Inherit(f =>
            {
                f.Enable = true;
            });

            var model = SPMeta2Model.NewFarmModel(farm =>
            {
                farm.AddFarmFeature(farmFeature);
            });

            DeployModel(model);
        }

        #endregion
    }
}