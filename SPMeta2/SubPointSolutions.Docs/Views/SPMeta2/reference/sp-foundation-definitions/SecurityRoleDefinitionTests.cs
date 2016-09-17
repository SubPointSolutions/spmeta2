using System.Collections.ObjectModel;
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
    public class SecurityRoleDefinitionTests : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.SecurityRoleDefinition")]

        [SampleMetadata(Title = "Add security role",
                            Description = ""
                            )]
        //[SampleMetadataTag(Name = BuiltInTagNames.SampleHidden)]

        public void CanDeploySimpleSecurityRoleDefinition()
        {
            var customerEditors = new SecurityRoleDefinition
            {
                Name = "Customer editors",
                BasePermissions = new Collection<string>
                {
                    BuiltInBasePermissions.EditListItems,
                    BuiltInBasePermissions.UseClientIntegration
                }
            };

            var customerApprovers = new SecurityRoleDefinition
            {
                Name = "Customer approvers",
                BasePermissions = new Collection<string>
                {
                    BuiltInBasePermissions.EditListItems,
                    BuiltInBasePermissions.DeleteListItems,
                    BuiltInBasePermissions.UseClientIntegration
                }
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddSecurityRole(customerEditors)
                    .AddSecurityRole(customerApprovers);
            });

            DeployModel(model);
        }

        #endregion
    }
}