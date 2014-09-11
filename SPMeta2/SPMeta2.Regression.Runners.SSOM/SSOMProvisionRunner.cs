using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Models;
using SPMeta2.Regression.Runners.Consts;
using SPMeta2.Regression.Runners.Utils;
using SPMeta2.Regression.SSOM;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Services;
using Microsoft.SharePoint.Administration;
using System.Web.UI.WebControls.WebParts;

namespace SPMeta2.Regression.Runners.SSOM
{
    public class SSOMProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        public SSOMProvisionRunner()
        {
            Name = "SSOM";

            WebApplicationUrl = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.SSOM_WebApplicationUrl);
            SiteUrl = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.SSOM_SiteUrl);
        }

        #endregion

        #region properties

        public string WebApplicationUrl { get; set; }
        public string SiteUrl { get; set; }

        private readonly SSOMProvisionService _provisionService = new SSOMProvisionService();
        private readonly SSOMValidationService _validationService = new SSOMValidationService();

        public override string ResolveFullTypeName(string typeName, string assemblyName)
        {
            var sp = typeof(SPField);
            var wp = typeof(WebPart);

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
            for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
            {
                WithSSOMWebApplicationContext(webApp =>
                {

                    _provisionService.DeployModel(WebApplicationModelHost.FromWebApplication(webApp), model);
                    _validationService.DeployModel(WebApplicationModelHost.FromWebApplication(webApp), model);

                });
            }
        }

        public override void DeploySiteModel(ModelNode model)
        {
            for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
            {
                WithSSOMSiteAndWebContext((site, web) =>
                {
                    _provisionService.DeployModel(SiteModelHost.FromSite(site), model);
                    _validationService.DeployModel(SiteModelHost.FromSite(site), model);
                });
            }
        }

        public override void DeployWebModel(ModelNode model)
        {
            for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
            {
                WithSSOMSiteAndWebContext((site, web) =>
                {
                    _provisionService.DeployModel(WebModelHost.FromWeb(web), model);
                    _validationService.DeployModel(WebModelHost.FromWeb(web), model);
                });
            }
        }

        #endregion

        #region utils

        private void WithSSOMWebApplicationContext(Action<SPWebApplication> action)
        {
            var webApp = SPWebApplication.Lookup(new Uri(WebApplicationUrl));

            action(webApp);
        }

        private void WithSSOMFarmContext(Action<SPFarm> action)
        {
            var farm = SPFarm.Local;

            action(farm);
        }

        private void WithSSOMSiteAndWebContext(Action<SPSite, SPWeb> action)
        {
            using (var site = new SPSite(SiteUrl))
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
