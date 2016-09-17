using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.CSOM.DefaultSyntax;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.WebParts)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]

    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class ContentEditorWebPartDefinitionTests : ProvisionTestBase
    {
        #region methods

       

        [TestMethod]
        [TestCategory("Docs.ContentEditorWebPartDefinition")]

        [SampleMetadata(Title = "Add CEWP",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploEmptyContentEditorWebpart()
        {
            var cewp = new ContentEditorWebPartDefinition
            {
                Title = "Empty Content Editor Webpart",
                Id = "m2EmptyCEWP",
                ZoneIndex = 10,
                ZoneId = "Main"
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 CEWP provision",
                FileName = "cewp-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddWebPartPage(webPartPage, page =>
                    {
                        page.AddContentEditorWebPart(cewp);
                    });
                });
            });

            DeployModel(model);
        }

        

        [TestMethod]
        [TestCategory("Docs.ContentEditorWebPartDefinition")]

        [SampleMetadata(Title = "Add CEWP with link",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploContentEditorWebpartWithUrlLink()
        {
            var htmlContent = new ModuleFileDefinition
            {
                FileName = "m2-cewp-content.html",
                Content = Encoding.UTF8.GetBytes("M2 is everything you need to deploy stuff to Sharepoint"),
                Overwrite = true,
            };

            var cewp = new ContentEditorWebPartDefinition
            {
                Title = "Content Editor Webpart with URL link",
                Id = "m2ContentLinkCEWP",
                ZoneIndex = 20,
                ZoneId = "Main",
                ContentLink = UrlUtility.CombineUrl(new string[]{
                        "~sitecollection",
                        BuiltInListDefinitions.StyleLibrary.GetListUrl(),
                        htmlContent.FileName})
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 CEWP provision",
                FileName = "cewp-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddHostList(BuiltInListDefinitions.StyleLibrary, list =>
                    {
                        list.AddModuleFile(htmlContent);
                    })
                    .AddHostList(BuiltInListDefinitions.SitePages, list =>
                    {
                        list.AddWebPartPage(webPartPage, page =>
                        {
                            page.AddContentEditorWebPart(cewp);
                        });
                    });
            });

            DeployModel(model);
        }


        [TestMethod]
        [TestCategory("Docs.ContentEditorWebPartDefinition")]

        [SampleMetadata(Title = "Add CEWP with content",
                        Description = ""
                        )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeployContentEditorWebpartWithContent()
        {
            var cewp = new ContentEditorWebPartDefinition
            {
                Title = "Content Editor Webpart with content",
                Id = "m2ContentCEWP",
                ZoneIndex = 30,
                ZoneId = "Main",
                Content = "Content Editor web part inplace content."
            };

            var webPartPage = new WebPartPageDefinition
            {
                Title = "M2 CEWP provision",
                FileName = "cewp-provision.aspx",
                PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddHostList(BuiltInListDefinitions.SitePages, list =>
                {
                    list.AddWebPartPage(webPartPage, page =>
                    {
                        page.AddContentEditorWebPart(cewp);
                    });
                });
            });

            DeployModel(model);
        }

        #endregion
    }
}