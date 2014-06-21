using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class WebTests : SPMeta2RegresionTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_BlankWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.BlankWeb));

            WithProvisionRunners(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_BlogWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.BlogWeb));

            WithProvisionRunners(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_DocumentCenterWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.DocumentCenterWeb));

            WithProvisionRunners(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_SearchCenterLightWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.SearchCenterLightWeb));

            WithProvisionRunners(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_SearchCenterWithTabsWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.SearchCenterWithTabsWeb));

            WithProvisionRunners(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_TeamWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.TeamWeb));

            WithProvisionRunners(runner => runner.DeployWebModel(model));
        }

        #endregion
    }
}
