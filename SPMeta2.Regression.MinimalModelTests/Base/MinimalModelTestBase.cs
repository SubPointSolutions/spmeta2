using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Services;
using SPMeta2.Models;
using SPMeta2.Regression.Base;
using SPMeta2.Regression.CSOM;
using SPMeta2.Regression.ModelBuilders;
using SPMeta2.Regression.SSOM;
using SPMeta2.SSOM.Services;

namespace SPMeta2.Regression.MinimalModelTests.Base
{
    public class MinimalModelTestBase : SPWebApplicationSandboxTest
    {
        #region contructors

        public MinimalModelTestBase()
        {
            ProvisionEngineSettings = new List<ProvisionEngineSettings>();

            ProvisionEngineSettings.Add(new ProvisionEngineSettings
            {
                Name = "SSOM Provision",
                ProvisionService = new SSOMProvisionService(),
                ValidationService = new SSOMValidationService(),
                IsEnabled = false,
                Scope = ProvisionEngineSettingsScope.SSOM_SPSite
            });

            ProvisionEngineSettings.Add(new ProvisionEngineSettings
            {
                Name = "CSOM Provision",
                ProvisionService = new CSOMProvisionService(),
                ValidationService = new CSOMValidationService(),
                IsEnabled = false,
                Scope = ProvisionEngineSettingsScope.CSOM_ClientContext
            });

            ProvisionEngineSettings.Add(new ProvisionEngineSettings
           {
               Name = "O365 Provision",
               ProvisionService = new CSOMProvisionService(),
               ValidationService = new CSOMValidationService(),
               IsEnabled = true,
               Scope = ProvisionEngineSettingsScope.O365_ClientContext
           });
        }

        #endregion

        #region properties

        public List<ProvisionEngineSettings> ProvisionEngineSettings { get; set; }

        public virtual string SiteCollectionTitle
        {
            get
            {
                return "minimal-model-app";
            }
        }

        protected DynamicModelBuilder _modelBuilder = new DynamicModelBuilder();

        protected virtual DynamicModelBuilder ModelBuilder
        {
            get { return _modelBuilder; }
        }


        public virtual void RunSiteModelTests(ModelNode model)
        {

        }

        public virtual void RunWebModelTests(ModelHostScope modelModelScope, ModelNode model)
        {
            foreach (var testScope in ProvisionEngineSettings.Where(s => s.IsEnabled))
            {
                switch (testScope.Scope)
                {
                    case ProvisionEngineSettingsScope.SSOM_SPSite:
                        {
                            base.WithStaticSPSiteSandbox(SiteCollectionTitle, (spSite, spWeb) =>
                            {
                                if (modelModelScope == ModelHostScope.SPSite)
                                    testScope.ProvisionService.DeployModel(spSite, model);

                                if (modelModelScope == ModelHostScope.SPWeb)
                                    testScope.ProvisionService.DeployModel(spWeb, model);
                            });
                        }

                        break;

                    case ProvisionEngineSettingsScope.CSOM_ClientContext:
                        {
                            base.WithStaticSPSiteSandbox(SiteCollectionTitle, (spSite, spWeb) =>
                            {
                                using (var context = new ClientContext(spWeb.Url))
                                {
                                    if (modelModelScope == ModelHostScope.SPSite)
                                        testScope.ProvisionService.DeployModel(context.Site, model);

                                    if (modelModelScope == ModelHostScope.SPWeb)
                                        testScope.ProvisionService.DeployModel(context.Web, model);
                                }

                            });
                        }

                        break;

                    case ProvisionEngineSettingsScope.O365_ClientContext:
                        {
                            WithO365Context((context) =>
                            {
                                if (modelModelScope == ModelHostScope.SPSite)
                                    testScope.ProvisionService.DeployModel(context.Site, model);

                                if (modelModelScope == ModelHostScope.SPWeb)
                                    testScope.ProvisionService.DeployModel(context.Web, model);
                            });
                        }

                        break;
                }
            }
        }

        #endregion
    }
}
