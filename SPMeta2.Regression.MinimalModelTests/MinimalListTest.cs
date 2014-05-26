using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.MinimalModelTests.Base;

namespace SPMeta2.Regression.MinimalModelTests
{
    [TestClass]
    public class MinimalListTest : MinimalModelTestBase
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
        public void CanDeployMinimalListSet()
        {
            var model = ModelBuilder.GetWebWithDefaultLists();

            RunWebModelTests(ModelHostScope.SPWeb, model);
        }

        [TestMethod]
        [TestCategory("MinimalModel")]
        public void CanDeployMinimalListWithContentTypeSet()
        {
            // deploy metadata first
            var metadataModel = ModelBuilder.GetSiteWithDefaultFieldsAndFilledContentTypes();
            var listModel = ModelBuilder.GetWebWithDefaultListsAndContentTypes();

            RunWebModelTests(ModelHostScope.SPSite, metadataModel);
            RunWebModelTests(ModelHostScope.SPWeb, listModel);
        }

        [TestMethod]
        [TestCategory("MinimalModel")]
        public void CanDeployMinimalListWithContentTypeAndViewsSet()
        {
            var metadataModel = ModelBuilder.GetSiteWithDefaultFieldsAndFilledContentTypes();
            var listModel = ModelBuilder.GetWebWithDefaultListsContentTypesAndViews();

            RunWebModelTests(ModelHostScope.SPSite, metadataModel);
            RunWebModelTests(ModelHostScope.SPWeb, listModel);
        }

        [TestMethod]
        [TestCategory("MinimalModel")]
        public void CanDeployMinimalListWithSecurityGroupsSet()
        {
            var metadataModel = ModelBuilder.GetSiteWithDefaultFieldsAndFilledContentTypes();

            var securityGroupModel = ModelBuilder.GetWebWithDefaultSecurityGroups();
            var securityRoleModel = ModelBuilder.GetWebWithDefaultSecurityRoles();

            var listModel = ModelBuilder.GetWebWithDefaultListsAndSecurityGroups();

            RunWebModelTests(ModelHostScope.SPSite, metadataModel);

            RunWebModelTests(ModelHostScope.SPWeb, securityGroupModel);
            RunWebModelTests(ModelHostScope.SPWeb, securityRoleModel);
            RunWebModelTests(ModelHostScope.SPWeb, listModel);
        }

        #endregion
    }
}
