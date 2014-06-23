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

namespace SPMeta2.Regression.Runners.SSOM
{
    public class SSOMProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        public SSOMProvisionRunner()
        {
            WebApplicationUrl = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.SSOM_WebApplicationUrl);
            SiteUrl = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.SSOM_SiteUrl);
        }

        #endregion

        #region properties

        public string WebApplicationUrl { get; set; }
        public string SiteUrl { get; set; }

        private readonly SSOMProvisionService _provisionService = new SSOMProvisionService();
        private readonly SSOMValidationService _validationService = new SSOMValidationService();


        #endregion

        #region methods

        public override void DeploySiteModel(ModelNode model)
        {
            WithSSOMContext((site, web) =>
            {
                _provisionService.DeployModel(SiteModelHost.FromSite(site), model);
                _validationService.DeployModel(SiteModelHost.FromSite(site), model);
            });
        }

        public override void DeployWebModel(ModelNode model)
        {
            WithSSOMContext((site, web) =>
            {
                _provisionService.DeployModel(WebModelHost.FromWeb(web), model);
                _validationService.DeployModel(WebModelHost.FromWeb(web), model);
            });
        }

        #endregion

        #region utils

        private void WithSSOMContext(Action<SPSite, SPWeb> action)
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
