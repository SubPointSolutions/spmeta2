using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Syntax.Default;

namespace SubPointSolutions.Docs.Views.Views.SPMeta2.reference
{
    [TestClass]
    public class WebModel : ProvisionTestBase
    {
        #region web model

        [TestMethod]
        [TestCategory("Docs.Models")]
        public void WebModelProvision()
        {
            // tend to separate models into small logical pieces
            // later you would deploy either all of them or only required bits

            var featuresModel = SPMeta2Model.NewWebModel(web =>
            {
                // setup features
            });

            var listsModel = SPMeta2Model.NewWebModel(web =>
            {
                // setup lists and list views
            });

            var pagesModel = SPMeta2Model.NewWebModel(web =>
            {
                // setup pages
            });

            var webPartsModel = SPMeta2Model.NewWebModel(web =>
            {
                // setup web parts
            });

            var navigationModel = SPMeta2Model.NewWebModel(web =>
            {
                // setup navigation 
            });

            // deploy needed models - all of them or only required bits
        }

        #endregion
    }
}