using Microsoft.VisualStudio.TestTools.UnitTesting;
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

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebApplication)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebApplicationModel)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategoryOrder, Value = "100")]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class ContentDatabaseDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.ContentDatabaseDefinition")]

        [SampleMetadata(Title = "Add content database",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleContentDatabaseDefinition()
        {
            var contentDb1 = new ContentDatabaseDefinition
            {
                ServerName = "localhost",
                DbName = "intranet_content_db1"
            };

            var contentDb2 = new ContentDatabaseDefinition
            {
                ServerName = "localhost",
                DbName = "intranet_content_db2"
            };

            var model = SPMeta2Model.NewWebApplicationModel(webApp =>
            {
                webApp
                    .AddContentDatabase(contentDb1)
                    .AddContentDatabase(contentDb2);
            });

            DeployModel(model);
        }

        #endregion
    }
}