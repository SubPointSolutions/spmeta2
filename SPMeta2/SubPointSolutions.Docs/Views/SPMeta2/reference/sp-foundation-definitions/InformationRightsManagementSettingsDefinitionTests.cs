using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTagAttribute(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.ListsAndLibraries)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]
    //[SampleMetadataTagAttribute(Name = BuiltInTagNames.SampleHidden)]
    public class InformationRightsManagementSettingsDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.InformationRightsManagementSettingsDefinition")]

        [SampleMetadata(Title = "Add IRM settings",
                        Description = ""
                        )]
        [SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleInformationRightsManagementSettingsDefinition()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
         
            });

            DeployModel(model);
        }

        #endregion
    }
}