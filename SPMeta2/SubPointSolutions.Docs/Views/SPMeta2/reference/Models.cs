using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using System;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    public class Models : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.Models")]
        public void SettingUpSiteModels()
        {
            // step 1, setup your definition

            // step 2, setup your site models
            var taxonomyModel = SPMeta2Model.NewSiteModel(site =>
            {
                // setup site taxonomy
            });

            var featuresAndSandboxSolutionsModel = SPMeta2Model.NewSiteModel(site =>
            {
                // setup sandbox solutions and features
            });

            var fieldsAndContentTypesModel = SPMeta2Model.NewSiteModel(site =>
            {
                // setup fields and content types
            });

            // step 3, deploy site models
        }

        [TestMethod]
        [TestCategory("Docs.Models")]
        public void SettingUpWebModels()
        {
            // step 1, setup your definition

            // step 2, setup your site models
            var featuresModel = SPMeta2Model.NewWebModel(web =>
            {
                // setup features
            });

            var listsModel = SPMeta2Model.NewWebModel(web =>
            {
                // setup fields and content types
            });

            var navigationModel = SPMeta2Model.NewWebModel(web =>
            {
                // setup web navigation
            });

            // step 3, deploy web models
        }

        #endregion
    }
}