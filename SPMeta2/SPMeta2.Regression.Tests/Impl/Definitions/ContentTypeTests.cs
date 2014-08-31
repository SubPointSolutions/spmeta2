using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Model.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    //[TestClass]
    public class ContentTypeTests : SPMeta2RegresionTestBase
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression")]
        public void CanDeployCustomDocumentContentType()
        {
            var model = SPMeta2Model
                .NewSiteModel(site => site.AddContentType(RegContentTypes.CustomDocument));

            //  WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        [TestMethod]
        [TestCategory("Regression")]
        public void CanDeployCustomListItemContentType()
        {
            var model = SPMeta2Model
                .NewSiteModel(site => site.AddContentType(RegContentTypes.CustomItem));

            //  WithProvisionRunnerContext(runner => runner.DeploySiteModel(model));
        }

        //[TestMethod]
        //[TestCategory("Regression")]
        //public void CanDeployContentTypesWithResourceFolders()
        //{
        //    throw new NotImplementedException();
        //}

        //[TestMethod]
        //[TestCategory("Regression")]
        //public void CanDeployContentTypesWithNestedParents()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
