using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Common;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.Services;
using SPMeta2.SSOM.Standard.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using System;
using SPMeta2.Services;

namespace SubPointSolutions.Docs.Views.Views.SPMeta2.reference
{
    [TestClass]
    public class ProvisionServices : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.ProvisionServices")]
        public void Deploy_SiteModel_CSOM()
        {
            // setup url
            var siteUrl = "";

            // create you model
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {

            });

            // setup client context
            using (var clientContext = new ClientContext(siteUrl))
            {
                // deploy site model with SharePoint Foundation CSOM API
                var foundationProvisionService = new CSOMProvisionService();
                foundationProvisionService.DeploySiteModel(clientContext, siteModel);

                // deploy site model with SharePoint Standard CSOM API
                var standardProvisionService = new StandardCSOMProvisionService();
                standardProvisionService.DeploySiteModel(clientContext, siteModel);
            }
        }

        [TestMethod]
        [TestCategory("Docs.ProvisionServices")]
        public void Deploy_WebModel_CSOM()
        {
            // setup url
            var webUrl = "";

            // create you model
            var webModel = SPMeta2Model.NewWebModel(web =>
            {

            });

            // setup client context
            using (var clientContext = new ClientContext(webUrl))
            {
                // deploy web model with SharePoint Foundation CSOM API
                var foundationProvisionService = new CSOMProvisionService();
                foundationProvisionService.DeployWebModel(clientContext, webModel);

                // deploy web model with SharePoint Standard CSOM API
                var standardProvisionService = new StandardCSOMProvisionService();
                standardProvisionService.DeployWebModel(clientContext, webModel);
            }
        }

        #region ssom

        [TestMethod]
        [TestCategory("Docs.ProvisionServices")]
        public void Deploy_FarmModel_SSOM()
        {
            // create you model
            var farmModel = SPMeta2Model.NewFarmModel(farm =>
            {

            });

            var spFarm = SPFarm.Local;

            // deploy site model with SharePoint Foundation SSOM API
            var foundationProvisionService = new SSOMProvisionService();
            foundationProvisionService.DeployFarmModel(spFarm, farmModel);

            // deploy site model with SharePoint Standard SSOM API
            var standardProvisionService = new StandardSSOMProvisionService();
            standardProvisionService.DeployFarmModel(spFarm, farmModel);
        }

        [TestMethod]
        [TestCategory("Docs.ProvisionServices")]
        public void Deploy_WebApplicationModel_SSOM()
        {
            // setup url
            var webAppUrl = "";

            // create you model
            var webAppModel = SPMeta2Model.NewWebApplicationModel(webApp =>
            {

            });

            var spWebApp = SPWebApplication.Lookup(new Uri(webAppUrl));

            // deploy site model with SharePoint Foundation SSOM API
            var foundationProvisionService = new SSOMProvisionService();
            foundationProvisionService.DeployWebApplicationModel(spWebApp, webAppModel);

            // deploy site model with SharePoint Standard SSOM API
            var standardProvisionService = new StandardSSOMProvisionService();
            standardProvisionService.DeployWebApplicationModel(spWebApp, webAppModel);
        }

        [TestMethod]
        [TestCategory("Docs.ProvisionServices")]
        public void Deploy_SiteModel_SSOM()
        {
            // setup url
            var siteUrl = "";

            // create you model
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {

            });

            using (var spSite = new SPSite(siteUrl))
            {
                // deploy site model with SharePoint Foundation SSOM API
                var foundationProvisionService = new SSOMProvisionService();
                foundationProvisionService.DeploySiteModel(spSite, siteModel);

                // deploy site model with SharePoint Standard SSOM API
                var standardProvisionService = new StandardSSOMProvisionService();
                standardProvisionService.DeploySiteModel(spSite, siteModel);
            }
        }

        [TestMethod]
        [TestCategory("Docs.ProvisionServices")]
        public void Deploy_WebModel_SSOM()
        {
            // setup url
            var webUrl = "";

            // create you model
            var webModel = SPMeta2Model.NewWebModel(web =>
            {

            });

            using (var spSite = new SPSite(webUrl))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    // deploy site model with SharePoint Foundation SSOM API
                    var foundationProvisionService = new SSOMProvisionService();
                    foundationProvisionService.DeployWebModel(spWeb, webModel);

                    // deploy site model with SharePoint Standard SSOM API
                    var standardProvisionService = new StandardSSOMProvisionService();
                    standardProvisionService.DeployWebModel(spWeb, webModel);
                }
            }
        }

        #endregion

        #region incremental

        [TestMethod]
        [TestCategory("Docs.ProvisionServices")]
        public void DeployIncrementally_Default_SSOM()
        {
            // setup url
            var siteUrl = "";

            // create you models
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {

            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {

            });

            using (var spSite = new SPSite(siteUrl))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    // deploy site model incrementally
                    var provisionService = new StandardSSOMProvisionService();
                    provisionService.DeploySiteModelIncrementally(spSite, siteModel, "MySiteModelId");

                    // deploy web model incrementally
                    provisionService.DeployWebModelIncrementally(spWeb, webModel, "MyWebModelId");
                }
            }
        }

        [TestMethod]
        [TestCategory("Docs.ProvisionServices")]
        public void DeployIncrementally_Default_CSOM()
        {
            // setup web
            var siteUrl = "";

            // create you models
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {

            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {

            });

            // setup client context
            using (var clientContext = new ClientContext(siteUrl))
            {
                // deploy site model incrementally
                var provisionService = new StandardCSOMProvisionService();
                provisionService.DeploySiteModelIncrementally(clientContext, siteModel, "MySiteModelId");

                // deploy web model incrementally
                provisionService.DeployWebModelIncrementally(clientContext, webModel, "MyWebModelId");
            }
        }


        [TestMethod]
        [TestCategory("Docs.ProvisionServices")]
        public void DeployIncrementally_NormalSetup_CSOM()
        {
            // setup web
            var siteUrl = "";

            // create you models
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {

            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {

            });

            // setup client context
            using (var clientContext = new ClientContext(siteUrl))
            {
                var provisionService = new StandardCSOMProvisionService();

                // setup incremental provision settings
                var incrementalProvisionConfig = new IncrementalProvisionConfig();

                // 1 - store model hash in SharePoint 
                incrementalProvisionConfig.AutoDetectSharePointPersistenceStorage = true;

                // 2 - set incremental provisio model for the provision service
                provisionService.SetIncrementalProvisionMode(incrementalProvisionConfig);

                // set model id for incremental provision
                siteModel.SetIncrementalProvisionModelId(incrementalModelId);
                webModel.SetIncrementalProvisionModelId(incrementalModelId);


                // deploy incrementally
                provisionService.DeploySiteModel(clientContext, siteModel);
                provisionService.DeployWebModel(clientContext, webModel);

                // revert back to normal provision mode
                provisionService.SetDefaultProvisionMode();
            }
        }

        #endregion

        #endregion
    }
}