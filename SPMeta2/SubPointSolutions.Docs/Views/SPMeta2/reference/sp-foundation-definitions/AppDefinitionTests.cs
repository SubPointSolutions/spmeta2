using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;
using System;
using System.IO;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]

    [SampleMetadataTagAttribute(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTagAttribute(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.SiteCollection)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]
    [SampleMetadataTagAttribute(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebSite)]

    //[SampleMetadataTagAttribute(Name = BuiltInTagNames.SampleHidden)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]

    public class AppDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.AppDefinition")]

        [SampleMetadata(Title = "Add app",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleAppDefinition()
        {
            var appDef = new AppDefinition
            {
                Content = File.ReadAllBytes("path-to-your-app-file"),
                ProductId = new Guid("your-app-product-id"),
                // your app version 
                Version = "1.0.0.0"
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddApp(appDef);
            });

            DeployModel(model);
        }

        #endregion
    }
}