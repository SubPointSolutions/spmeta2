using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;

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

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.SiteCollection)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class WebDefinitionTest : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.WebDefinition")]


        [SampleMetadata(Title = "Add web",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleWeb()
        {
            var newCustomerWeb = new WebDefinition
            {
                Title = "New customer site",
                Description = "A dedicated site for the customer support.",
                Url = "new-customer-web",
                WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite
            };
            var newPublishingWeb = new WebDefinition
            {
                Title = "Temporary Publishing Web",
                Description = "A temporary punlishing web.",
                Url = "new-publishing-web",
                WebTemplate = BuiltInWebTemplates.Publishing.PublishingPortal
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(newCustomerWeb);
                web.AddWeb(newPublishingWeb);
            });

            DeployModel(model);
        }


        [TestMethod]
        [TestCategory("Docs.WebDefinition")]
        [SampleMetadata(Title = "Add multiple webs",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleWebs()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(DocWebs.News);
                web.AddWeb(DocWebs.AboutOurCompany);
            });

            DeployModel(model);
        }



        [TestMethod]
        [TestCategory("Docs.WebDefinition")]

        [SampleMetadata(Title = "Add hierarchical webs",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]

        public void CanDeployHierarchicalWebs()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddWeb(DocWebs.News)
                    .AddWeb(DocWebs.Departments, departmentWeb =>
                    {
                        departmentWeb
                            .AddWeb(DocWebs.DepartmentWebs.HR)
                            .AddWeb(DocWebs.DepartmentWebs.ITHelpDesk, itWeb =>
                            {
                                itWeb
                                    .AddWeb(DocWebs.DepartmentWebs.ITHelpDeskWebs.Apple)
                                    .AddWeb(DocWebs.DepartmentWebs.ITHelpDeskWebs.Cisco)
                                    .AddWeb(DocWebs.DepartmentWebs.ITHelpDeskWebs.Microsoft);
                            })
                            .AddWeb(DocWebs.DepartmentWebs.Sales);
                    })
                    .AddWeb(DocWebs.AboutOurCompany);
            });

            DeployModel(model);
        }

        #endregion
    }
}
