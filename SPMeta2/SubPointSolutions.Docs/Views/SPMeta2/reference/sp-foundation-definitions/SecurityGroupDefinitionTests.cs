using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;

using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Syntax.Default;
using SubPointSolutions.Docs.Code.Definitions;
using SubPointSolutions.Docs.Code.Enumerations;
using SubPointSolutions.Docs.Code.Metadata;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]

    [SampleMetadataTag(Name = BuiltInTagNames.SPRuntime, Value = BuiltInSPRuntimeTagValues.Foundation)]

    [SampleMetadataTag(Name = BuiltInTagNames.SampleCategory, Value = BuiltInSampleCategoryTagValues.Security)]
    [SampleMetadataTag(Name = BuiltInTagNames.SampleM2Model, Value = BuiltInM2ModelTagValues.SiteModel)]
    //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
    public class SecurityGroupDefinitionTests : ProvisionTestBase
    {
        #region methods

       
        [TestMethod]
        [TestCategory("Docs.SecurityGroupDefinition")]

        [SampleMetadata(Title = "Add security group",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleSecurityGroup()
        {
            var auditors = new SecurityGroupDefinition
            {
                Name = "External Auditors",
                Description = "External auditors group."
            };

            var reviewers = new SecurityGroupDefinition
            {
                Name = "External Reviewers",
                Description = "External reviewers group."
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddSecurityGroup(auditors)
                    .AddSecurityGroup(reviewers);
            });

            DeployModel(model);
        }

      

        [TestMethod]
        [TestCategory("Docs.SecurityGroupDefinition")]
        [SampleMetadata(Title = "Add multiple security groups",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]
        public void CanDeploySimpleSecurityGroups()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddSecurityGroup(DocSecurityGroups.ClientManagers)
                    .AddSecurityGroup(DocSecurityGroups.ClientSupport)
                    .AddSecurityGroup(DocSecurityGroups.Interns)
                    .AddSecurityGroup(DocSecurityGroups.OrderApprovers);
            });

            DeployModel(model);
        }

        #endregion
    }
}
