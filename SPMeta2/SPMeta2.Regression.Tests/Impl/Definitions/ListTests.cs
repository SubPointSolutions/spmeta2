using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    //[TestClass]
    public class ListTests : SPMeta2RegresionTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Lists")]
        public void CanProvision_DocumentLibrary()
        {
            var model = SPMeta2Model
              .NewWebModel(site => site.AddList(RegLists.DocumentLibrary));

            WithProvisionRunners(runner => runner.DeployWebModel(model));
        }

        [TestMethod]
        [TestCategory("Regression.Lists")]
        public void CanProvision_GeneraicList()
        {
            var model = SPMeta2Model
              .NewWebModel(site => site.AddList(RegLists.GenericList));

            WithProvisionRunners(runner => runner.DeployWebModel(model));
        }

        #endregion
    }
}
