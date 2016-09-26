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

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.Navigation)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.WebModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class TopNavigationNodeDefinitionTests : ProvisionTestBase
    {
        [TestMethod]
        [TestCategory("Docs.TopNavigationNodeDefinition")]

        [SampleMetadata(Title = "Add top nav items",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CaDeployFlatTopNavigation()
        {
            var ourCompany = new TopNavigationNodeDefinition
            {
                Title = "Our Company",
                Url = "our-company.aspx",
                IsExternal = true
            };

            var ourServices = new TopNavigationNodeDefinition
            {
                Title = "Our Services",
                Url = "our-services.aspx",
                IsExternal = true
            };

            var ourTeam = new TopNavigationNodeDefinition
            {
                Title = "Our Team",
                Url = "our-team.aspx",
                IsExternal = true
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddTopNavigationNode(ourCompany)
                    .AddTopNavigationNode(ourServices)
                    .AddTopNavigationNode(ourTeam);
            });

            DeployModel(model);
        }


        [TestMethod]
        [TestCategory("Docs.TopNavigationNodeDefinition")]

        [SampleMetadata(Title = "Add hierarchical top nav items",
                    Description = ""
                    )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CaDeployHierarchicalTopNavigation()
        {
            // top level departments node
            var departments = new TopNavigationNodeDefinition
            {
                Title = "Our Departments",
                Url = "our-departments.aspx",
                IsExternal = true
            };

            var hr = new TopNavigationNodeDefinition
            {
                Title = "HR Team",
                Url = "hr-team.aspx",
                IsExternal = true
            };

            var it = new TopNavigationNodeDefinition
            {
                Title = "IT Team",
                Url = "it-team.aspx",
                IsExternal = true
            };

            // top level clients node
            var partners = new TopNavigationNodeDefinition
            {
                Title = "Our Partners",
                Url = "our-partners.aspx",
                IsExternal = true
            };

            var microsoft = new TopNavigationNodeDefinition
            {
                Title = "Microsoft",
                Url = "microsfot.aspx",
                IsExternal = true
            };

            var apple = new TopNavigationNodeDefinition
            {
                Title = "Apple",
                Url = "apple.aspx",
                IsExternal = true
            };

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddTopNavigationNode(departments, node =>
                    {
                        node
                            .AddTopNavigationNode(hr)
                            .AddTopNavigationNode(it);
                    })
                    .AddTopNavigationNode(partners, node =>
                    {
                        node
                          .AddTopNavigationNode(microsoft)
                          .AddTopNavigationNode(apple);
                    });
            });

            DeployModel(model);
        }
    }
}