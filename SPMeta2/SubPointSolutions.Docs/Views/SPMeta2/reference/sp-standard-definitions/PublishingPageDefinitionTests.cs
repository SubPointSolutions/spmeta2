using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]

    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Standard)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.PublishingPages)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class PublishingPageDefinitionTests : ProvisionTestBase
    {
        #region methods

       
        [TestMethod]
        [TestCategory("Docs.PublishingPageDefinition")]
        [SampleMetadata(Title = "Add publishing pages",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployPublishingPage()
        {
            var aboutPublishing = new PublishingPageDefinition
            {
                Title = "About publishing",
                FileName = "About-publishing.aspx",
                PageLayoutFileName = BuiltInPublishingPageLayoutNames.ArticleLeft
            };

            var howToPublising = new PublishingPageDefinition
            {
                Title = "How to publish",
                FileName = "How-to-publish.aspx",
                PageLayoutFileName = BuiltInPublishingPageLayoutNames.ArticleRight
            };

            var publishingLinks = new PublishingPageDefinition
            {
                Title = "Publishing links",
                FileName = "Publishing-links.aspx",
                PageLayoutFileName = BuiltInPublishingPageLayoutNames.ArticleLinks
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.Pages, list =>
                {
                    list
                        .AddPublishingPage(aboutPublishing)
                        .AddPublishingPage(howToPublising)
                        .AddPublishingPage(publishingLinks);
                });
            });

            DeployModel(model);
        }

       
        [TestMethod]
        [TestCategory("Docs.PublishingPageDefinition")]
        [SampleMetadata(Title = "Add publishing pages with custom layout",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployPublishingPageWithCustomeLayout()
        {
            // PageLayoutFileName allows you to setup your own publishing page layout file name
            // it should be a file name of the file inside 'master page' gallery

            var customPublishing = new PublishingPageDefinition
            {
                Title = "Custom publishing",
                FileName = "Custom-publishing.aspx",
                PageLayoutFileName = "__ specify a publishing page layout file name here ___"
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.Pages, list =>
                {
                    list
                        .AddPublishingPage(customPublishing);
                });
            });

            DeployModel(model);
        }

     
        [TestMethod]
        [TestCategory("Docs.PublishingPageDefinition")]
        [SampleMetadata(Title = "Add publishing pages to folders",
            Description = ""
            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployPublishingPageToFolders()
        {
            var archive = new FolderDefinition()
            {
                Name = "Archive"
            };

            var year2014 = new FolderDefinition()
            {
                Name = "2014"
            };

            var year2015 = new FolderDefinition()
            {
                Name = "2015"
            };

            var oct2014Article = new PublishingPageDefinition
            {
                Title = "October 2014",
                FileName = "october-2014.aspx",
                PageLayoutFileName = BuiltInPublishingPageLayoutNames.ArticleLeft
            };

            var dec2014Article = new PublishingPageDefinition
            {
                Title = "December 2014",
                FileName = "december-2014.aspx",
                PageLayoutFileName = BuiltInPublishingPageLayoutNames.ArticleLeft
            };

            var oct2015Article = new PublishingPageDefinition
            {
                Title = "October 2015",
                FileName = "october-2015.aspx",
                PageLayoutFileName = BuiltInPublishingPageLayoutNames.ArticleLeft
            };

            var dec2015Article = new PublishingPageDefinition
            {
                Title = "December 2015",
                FileName = "december-2015.aspx",
                PageLayoutFileName = BuiltInPublishingPageLayoutNames.ArticleLeft
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.Pages, list =>
                {
                    list.AddFolder(archive, folder =>
                    {
                        folder
                            .AddFolder(year2014, archive2014 =>
                            {
                                archive2014
                                    .AddPublishingPage(oct2014Article)
                                    .AddPublishingPage(dec2014Article);
                            })
                            .AddFolder(year2015, archive2015 =>
                            {
                                archive2015
                                   .AddPublishingPage(oct2015Article)
                                   .AddPublishingPage(dec2015Article);
                            });
                    });
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}