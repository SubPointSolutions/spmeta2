using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    //[TestClass]
    public class WebTests : SPMeta2RegresionTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_BlankWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.BlankWeb));

            //WithProvisionRunnerContext(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_BlogWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.BlogWeb));

            // WithProvisionRunnerContext(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_DocumentCenterWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.DocumentCenterWeb));

            //WithProvisionRunnerContext(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_SearchCenterLightWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.SearchCenterLightWeb));

            // WithProvisionRunnerContext(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_SearchCenterWithTabsWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.SearchCenterWithTabsWeb));

            //WithProvisionRunnerContext(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Webs")]
        public void CanProvision_TeamWeb()
        {
            var model = SPMeta2Model
                 .NewWebModel(web => web.AddWeb(RegWebs.TeamWeb));

            // WithProvisionRunnerContext(runner => runner.DeployWebModel(model));
        }

        #endregion
    }
}
