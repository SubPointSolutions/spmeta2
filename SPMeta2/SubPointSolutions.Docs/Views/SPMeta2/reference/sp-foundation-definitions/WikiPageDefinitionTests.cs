using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Definitions;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WikiPages)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class WikiPageDefinitionTests : ProvisionTestBase
    {
        #region methods

      

        [TestMethod]
        [TestCategory("Docs.WikiPageDefinition")]

        [SampleMetadata(Title = "Add wiki pages",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleWikiPages()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddWikiPage(DocWikiPages.AboutUs)
                        .AddWikiPage(DocWikiPages.Contacts);
                });
            });

            DeployModel(model);
        }

      

        [TestMethod]
        [TestCategory("Docs.WikiPageDefinition")]

        [SampleMetadata(Title = "Add wiki pages to folders",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployWikiPagesUnderFolder()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list
                        .AddFolder(DocFolders.WikiPages.News, newsFolder =>
                        {
                            newsFolder
                                .AddWikiPage(DocWikiPages.NewCoffeeMachine)
                                .AddWikiPage(DocWikiPages.NewSPMeta2Release);
                        })
                        .AddFolder(DocFolders.WikiPages.Archive, archiveFolder =>
                        {
                            archiveFolder
                               .AddWikiPage(DocWikiPages.December2012News)
                               .AddWikiPage(DocWikiPages.October2012News);
                        });
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}
