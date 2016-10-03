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

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.Security)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]

    public class SecurityGroupLinkDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.SecurityGroupLinkDefinition")]

        [SampleMetadata(Title = "Assign security group to web",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleSecurityGroupLinkDefinitionToWeb()
        {
            var auditors = new SecurityGroupDefinition
            {
                Name = "External Auditors",
                Description = "External auditors group."
            };

            // add group to the site first
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroup(auditors);
            });

            // assign group to the web, via .AddSecurityGroupLink() method
            var webModel = SPMeta2Model.NewWebModel(web =>
            {

                web.AddSecurityGroupLink(auditors);
            });

            DeployModel(siteModel);
            DeployModel(webModel);
        }

        [SampleMetadata(Title = "Assign security group to list",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleSecurityGroupLinkDefinitionToList()
        {
            var auditors = new SecurityGroupDefinition
            {
                Name = "External Auditors",
                Description = "External auditors group."
            };

            var auditorsList = new ListDefinition
            {
                Title = "Auditors documents",
                TemplateType = BuiltInListTemplateTypeId.DocumentLibrary,
                CustomUrl = "audit-docs"
            };

            // add group to the site first
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroup(auditors);
            });

            // assign group to the list, via .AddSecurityGroupLink() method
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(auditorsList, list =>
                {
                    list.AddSecurityGroupLink(auditors);
                });
            });

            DeployModel(siteModel);
            DeployModel(webModel);
        }

        #endregion
    }
}