using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.MinimalModelTests.Base;

namespace SPMeta2.Regression.MinimalModelTests
{
    [TestClass]
    public class MinimalSecurityGroupTest : MinimalModelTestBase
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
        public void CanDeployMinimalSecurityGroupSet()
        {
            var model = ModelBuilder.GetWebWithDefaultSecurityGroups();

            RunWebModelTests(ModelHostScope.SPWeb, model);

         }

        #endregion
    }
}
