using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Tests
{
    //[TestClass]
    public class SecurityGroupModelTests : ClientOMSharePointTestBase
    {
        #region resources

        [TestInitialize]
        public void Setup()
        {
            // it is a good place to change TestSetting
            InitTestSettings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupResources();
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeploySecurityModels()
        {
            var model = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                                       .WithSecurityRoles(roles =>
                                       {
                                           roles
                                               .AddSecurityRole(SecurityRoleModels.AnnouncmentsRole)
                                               .AddSecurityRole(SecurityRoleModels.ContractorRole);
                                       })
                                        .WithSecurityGroups(groups =>
                                        {
                                            groups
                                                .AddSecurityGroup(SecurityGroupModels.AnnounsmentsEditors, group =>
                                                {
                                                    group
                                                        .AddSecurityRoleLink(SecurityRoleModels.AnnouncmentsRole)
                                                        .AddSecurityRoleLink(SecurityRoleModels.ContractorRole);
                                                })
                                                .AddSecurityGroup(SecurityGroupModels.Contractors, group =>
                                                {
                                                    group
                                                        .AddSecurityRoleLink(SecurityRoleModels.ContractorRole);
                                                })
                                                .AddSecurityGroup(SecurityGroupModels.Students);
                                        });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(SiteModelHost.FromClientContext(context), model);
            });
        }

        #endregion
    }
}
