using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Office.SecureStoreService.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Portability;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Publishing.Navigation;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.WorkflowServices;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Utils;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using SPMeta2.Regression.CSOM.Validation.Webparts;
using SPMeta2.Regression.SSOM;
using SPMeta2.Regression.SSOM.Standard.Validation;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Services;
using SPMeta2.SSOM.Standard.ModelHandlers.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.Containers.SSOM
{
    public class SSOMProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        public SSOMProvisionRunner()
        {
            Name = "SSOM";

            WebApplicationUrls = new List<string>();
            SiteUrls = new List<string>();
            WebUrls = new List<string>();

            LoadEnvironmentConfig();
            InitServices();
        }

        private void InitServices()
        {
            _provisionService = new SSOMProvisionService();
            _validationService = new SSOMValidationService();

            var ssomStandartAsm = typeof(ContactFieldControlModelHandler).Assembly;

            foreach (var handlerType in ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(ssomStandartAsm))
                _provisionService.RegisterModelHandler(Activator.CreateInstance(handlerType) as ModelHandlerBase);

            var ssomStandartValidationAsm = typeof(ImageRenditionDefinitionValidator).Assembly;

            foreach (var handlerType in ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(ssomStandartValidationAsm))
                _validationService.RegisterModelHandler(Activator.CreateInstance(handlerType) as ModelHandlerBase);
        }

        private void LoadEnvironmentConfig()
        {
            WebApplicationUrls.Clear();
            WebApplicationUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.SSOM_WebApplicationUrls));

            SiteUrls.Clear();
            SiteUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.SSOM_SiteUrls));

            WebUrls.Clear();
            WebUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.SSOM_WebUrls));
        }

        #endregion

        #region properties

        public List<string> WebApplicationUrls { get; set; }
        public List<string> SiteUrls { get; set; }
        public List<string> WebUrls { get; set; }

        private SSOMProvisionService _provisionService;
        private SSOMValidationService _validationService;

        public override string ResolveFullTypeName(string typeName, string assemblyName)
        {
            var sp = typeof(SPField);
            var wp = typeof(WebPart);
            var publishing = typeof(WebNavigationSettings);

            var tax = typeof(TaxonomyField);
            var wf = typeof(WorkflowDefinition);

            var secureStore = typeof(ISecureStore);

            var search = typeof(SearchConfigurationPortability);

            var up = typeof(Audience);

            return base.ResolveFullTypeName(typeName, assemblyName);
        }

        #endregion

        #region methods

        public override void DeployFarmModel(ModelNode model)
        {
            for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
            {
                WithSSOMFarmContext(farm =>
                {
                    if (EnableDefinitionProvision)
                        _provisionService.DeployModel(FarmModelHost.FromFarm(farm), model);

                    if (EnableDefinitionValidation)
                        _validationService.DeployModel(FarmModelHost.FromFarm(farm), model);
                });
            }
        }

        public override void DeployWebApplicationModel(ModelNode model)
        {
            foreach (var webAppUrl in WebApplicationUrls)
            {
                Trace.WriteLine(string.Format("[INF]    Running on web app: [{0}]", webAppUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithSSOMWebApplicationContext(webAppUrl, webApp =>
                    {
                        if (EnableDefinitionProvision)
                            _provisionService.DeployModel(WebApplicationModelHost.FromWebApplication(webApp), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(WebApplicationModelHost.FromWebApplication(webApp), model);

                    });
                }
            }
        }

        public override void DeploySiteModel(ModelNode model)
        {
            foreach (var siteUrl in SiteUrls)
            {
                Trace.WriteLine(string.Format("[INF]    Running on site: [{0}]", siteUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithSSOMSiteAndWebContext(siteUrl, (site, web) =>
                    {
                        if (EnableDefinitionProvision)
                            _provisionService.DeployModel(SiteModelHost.FromSite(site), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(SiteModelHost.FromSite(site), model);
                    });
                }
            }
        }

        public override void DeployWebModel(ModelNode model)
        {
            foreach (var webUrl in WebUrls)
            {
                Trace.WriteLine(string.Format("[INF]    Running on web: [{0}]", webUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithSSOMSiteAndWebContext(webUrl, (site, web) =>
                    {
                        if (EnableDefinitionProvision)
                            _provisionService.DeployModel(WebModelHost.FromWeb(web), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(WebModelHost.FromWeb(web), model);
                    });
                }
            }
        }

        #endregion

        #region utils

        private void WithSSOMWebApplicationContext(string webAppUrl, Action<SPWebApplication> action)
        {
            var webApp = SPWebApplication.Lookup(new Uri(webAppUrl));

            action(webApp);
        }

        private void WithSSOMFarmContext(Action<SPFarm> action)
        {
            var farm = SPFarm.Local;

            action(farm);
        }

        private void WithSSOMSiteAndWebContext(string siteUrl, Action<SPSite, SPWeb> action)
        {
            using (var site = new SPSite(siteUrl))
            {
                using (var web = site.OpenWeb())
                {
                    action(site, web);
                }
            }
        }

        #endregion
    }
}
