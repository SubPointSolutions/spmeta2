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

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.EventReceivers)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class EventReceiverDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.EventReceiverDefinition")]

        [SampleMetadata(Title = "Add event receiver to site",
                        Description = ""
                        )]
        [SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySiteEventReceiverDefinition()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
         
            });

            DeployModel(model);
        }

        [SampleMetadata(Title = "Add event receiver to web",
                        Description = ""
                        )]
        [SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployWebSimpleEventReceiverDefinition()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {

            });

            DeployModel(model);
        }

        [SampleMetadata(Title = "Add event receiver to list",
                        Description = ""
                        )]
        [SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployListSimpleEventReceiverDefinition()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {

            });

            DeployModel(model);
        }

        [SampleMetadata(Title = "Add event receiver to content type",
                        Description = ""
                        )]
        [SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployContentTypeSimpleEventReceiverDefinition()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {

            });

            DeployModel(model);
        }

        #endregion
    }
}