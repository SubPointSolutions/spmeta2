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
    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategoryOrder, Value = "200")]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebApplicationModel)]

    public class AlternateUrlDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.AlternateUrlDefinition")]

        [SampleMetadata(Title = "Add alternate URL",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleAlternateUrlDefinition()
        {
            var internalDef = new AlternateUrlDefinition
            {
                Url = "http://the-portal",
                UrlZone = BuiltInUrlZone.Intranet
            };

            var intranetDef = new AlternateUrlDefinition
            {
                Url = "http://my-intranet.com.au",
                UrlZone = BuiltInUrlZone.Internet
            };

            var model = SPMeta2Model.NewWebApplicationModel(webApp =>
            {
                webApp.AddAlternateUrl(internalDef);
                webApp.AddAlternateUrl(intranetDef);
            });

            DeployModel(model);
        }

        #endregion
    }
}