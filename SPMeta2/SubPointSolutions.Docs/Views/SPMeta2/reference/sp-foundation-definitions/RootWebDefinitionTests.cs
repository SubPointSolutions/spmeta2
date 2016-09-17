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

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.RootWeb)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class RootWebDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.RootWebDefinition")]

        [SampleMetadata(Title = "Update root web Title/Description",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanUpdateRootWebProperties()
        {
            var rootWeb = new RootWebDefinition
            {
                Title = "M2 CRM",
                Description = "Custom CRM application build on top of M2 framework."
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRootWeb(rootWeb);
            });

            DeployModel(model);
        }

     

        [TestMethod]
        [TestCategory("Docs.RootWebDefinition")]
        [SampleMetadata(Title = "Add lists to root web",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanProvisionRootWebLists()
        {
            var rootWeb = new RootWebDefinition
            {

            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRootWeb(rootWeb, web =>
                {
                    web
                      .AddHostList(BuiltInListDefinitions.StyleLibrary, list =>
                      {
                          // do stuff with 'Style Library'
                      })
                      .AddHostList(BuiltInListDefinitions.Catalogs.MasterPage, list =>
                      {
                          // do stuff with 'Master Page Library'
                      });
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}