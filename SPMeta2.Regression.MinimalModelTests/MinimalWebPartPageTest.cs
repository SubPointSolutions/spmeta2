using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.MinimalModelTests.Base;

namespace SPMeta2.Regression.MinimalModelTests
{
    [TestClass]
    public class MinimalWebPartPageTest : MinimalModelTestBase
    {
        #region resources

        [TestInitialize]
        public void Setup()
        {
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
        [TestCategory("MinimalModel")]
        public void CanDeployMinimalWebPartPageSet()
        {
            var model = ModelBuilder.GetWebWithDefaultWebPartPages();

            RunWebModelTests(ModelHostScope.SPWeb, model);
        }

        [TestMethod]
        [TestCategory("MinimalModel")]
        public void CanDeployMinimalWebPartPageWithSecuritySet()
        {
            var securityGroupModel = ModelBuilder.GetWebWithDefaultSecurityGroups();
            var securityRoleModel = ModelBuilder.GetWebWithDefaultSecurityRoles();

            var webpartPagesModel = ModelBuilder.GetWebWithDefaultWebPartPagesWithSecurity();

            RunWebModelTests(ModelHostScope.SPWeb, securityGroupModel);
            RunWebModelTests(ModelHostScope.SPWeb, securityRoleModel);
            RunWebModelTests(ModelHostScope.SPWeb, webpartPagesModel);
        }

        #endregion
    }
}
