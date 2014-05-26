using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.MinimalModelTests.Base;

namespace SPMeta2.Regression.MinimalModelTests
{
    [TestClass]
    public class MinimalContentTypeTest : MinimalModelTestBase
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
        public void CanDeployMinimalContentTypeSet()
        {
            var model = ModelBuilder.GetSiteWithDefaultFieldsAndContentTypes();

            RunWebModelTests(ModelHostScope.SPSite, model);
        }

        [TestMethod]
        [TestCategory("MinimalModel")]
        public void CanDeployMinimalContentTypeFilledSet()
        {
            var model = ModelBuilder.GetSiteWithDefaultFieldsAndFilledContentTypes();

            RunWebModelTests(ModelHostScope.SPSite, model);
        }

        #endregion
    }
}
