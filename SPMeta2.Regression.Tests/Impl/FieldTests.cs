using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Model;
using SPMeta2.Regression.Tests.Base;

namespace SPMeta2.Regression.Tests.Impl
{
    [TestClass]
    public class FieldTests : SPMeta2RegresionTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression")]
        public void CanDeployFieldsToSite()
        {
            var model = new RegModel().GetSiteFields();

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression")]
        public void CanDeployFieldsToList()
        {
            var model = new RegModel().GetSiteFields();

            WithProvisionRunners(runner => runner.DeploySiteModel(model));
        }

        #endregion
    }
}
