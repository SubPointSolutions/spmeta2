using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]

    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebSite)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class MasterPageSettingsDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.MasterPageSettingsDefinition")]

        [SampleMetadata(Title = "Add master page setting",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployWebmasterPageSettings()
        {
            // BuiltInMasterPageDefinitions class could be used to refer OOTB master pages
            // BuiltInMasterPageDefinitions.Seattle 
            // BuiltInMasterPageDefinitions.Oslo  
            // BuiltInMasterPageDefinitions.Minimal  

            var masterPageSettings = new MasterPageSettingsDefinition
            {
                // both should be site relative URLs
                SiteMasterPageUrl = "/_catalogs/masterpage/oslo.master",
                SystemMasterPageUrl = "/_catalogs/masterpage/oslo.master"
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddMasterPageSettings(masterPageSettings);
            });

            DeployModel(model);
        }

        #endregion
    }
}