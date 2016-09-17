using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    public class SiteModel : ProvisionTestBase
    {
        #region site model

        [TestMethod]
        [TestCategory("Docs.Models")]
        public void SiteModelProvision()
        {
            // tend to separate models into small logical pieces
            // later you would deploy either all of them or only required bits

            var taxonomyModel = SPMeta2Model.NewSiteModel(site =>
            {
                // setup taxonomy
            });

            var sandboxSolutionsModel = SPMeta2Model.NewSiteModel(site =>
            {
                // setup sandbox solutions
            });

            var siteFeaturesModel = SPMeta2Model.NewSiteModel(site =>
            {
                // setup features
            });

            var siteIAModel = SPMeta2Model.NewSiteModel(site =>
            {
                // setup IA
            });

            var userCustomActionsModel = SPMeta2Model.NewSiteModel(site =>
            {
                // setup user custom actions model
            });


            // deploy needed models - all of them or only required bits
        }

        #endregion
    }
}