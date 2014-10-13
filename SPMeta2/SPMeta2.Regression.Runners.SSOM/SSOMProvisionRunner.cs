using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.Navigation;
using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Definitions.Fields;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using SPMeta2.Regression.Runners.Consts;
using SPMeta2.Regression.Runners.Utils;
using SPMeta2.Regression.SSOM;
using SPMeta2.Regression.SSOM.Standard.Validation.Webparts;
using SPMeta2.SSOM.ModelHandlers.Webparts;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Services;
using Microsoft.SharePoint.Administration;
using System.Web.UI.WebControls.WebParts;
using System.Diagnostics;
using SPMeta2.SSOM.Standard.ModelHandlers.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Runners.SSOM
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

            var ssomStandartValidationAsm = typeof(SiteFeedWebPartDefinitionValidator).Assembly;

            foreach (var handlerType in ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(ssomStandartValidationAsm))
                _validationService.RegisterModelHandler(Activator.CreateInstance(handlerType) as ModelHandlerBase);
        }

        private void LoadEnvironmentConfig()
        {
            WebApplicationUrls.Clear();
            WebApplicationUrls.AddRange(RunnerEnvironment.GetEnvironmentVariables(EnvironmentConsts.SSOM_WebApplicationUrls));

            SiteUrls.Clear();
            SiteUrls.AddRange(RunnerEnvironment.GetEnvironmentVariables(EnvironmentConsts.SSOM_SiteUrls));

            WebUrls.Clear();
            WebUrls.AddRange(RunnerEnvironment.GetEnvironmentVariables(EnvironmentConsts.SSOM_WebUrls));
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
            var publishing = typeof (WebNavigationSettings);

            var tax = typeof(TaxonomyField);

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
                    _provisionService.DeployModel(FarmModelHost.FromFarm(farm), model);
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

                        _provisionService.DeployModel(WebApplicationModelHost.FromWebApplication(webApp), model);
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
                        _provisionService.DeployModel(SiteModelHost.FromSite(site), model);
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
                        _provisionService.DeployModel(WebModelHost.FromWeb(web), model);
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
