using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Services;
using SPMeta2.Models;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.Services;
using SPMeta2.SSOM.Services;
using SPMeta2.Containers.Utils;
using SPMeta2.Containers.Consts;
using SPMeta2.Exceptions;

namespace SPMeta2.Docs.ProvisionSamples.Base
{
    public class ProvisionTestBase
    {
        #region constructors

        public ProvisionTestBase()
        {
            SSOMSiteUrl = RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.SSOM_SiteUrls).First();
            CSOMSiteUrl = RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.CSOM_SiteUrls).First();

            EnableCSOM = true;
            EnableSSOM = true;

            if (EnableSSOM)
            {
                if (!Environment.Is64BitProcess)
                {
                    throw new SPMeta2Exception("Environment.Is64BitProcess is false. If you run unit tests from Visual Studio, ensure 'Test -> Test Setting -> Default Processor Architecture -> x64'");
                }
            }
        }

        #endregion

        #region properties

        public bool EnableCSOM { get; set; }
        public bool EnableSSOM { get; set; }

        public string CSOMSiteUrl { get; set; }
        public string SSOMSiteUrl { get; set; }

        #endregion

        #region methods

        public void DeployModel(ModelNode model)
        {
            if (EnableCSOM)
            {
                DeployCSOMModel(model);
            }

            if (EnableSSOM)
            {
                DeploySSOMModel(model);
            }
        }

        protected void DeploySSOMModel(ModelNode model)
        {
            // deploy with SSOM
            var ssomProvisionService = new StandardSSOMProvisionService();

            using (var spSite = new SPSite(SSOMSiteUrl))
            {
                if (model.Value is FarmDefinition)
                    ssomProvisionService.DeployModel(new FarmModelHost(SPFarm.Local), model);

                if (model.Value is WebApplicationDefinition)
                    ssomProvisionService.DeployModel(new WebApplicationModelHost(spSite.WebApplication), model);

                if (model.Value is SiteDefinition)
                    ssomProvisionService.DeploySiteModel(spSite, model);

                if (model.Value is WebDefinition)
                    ssomProvisionService.DeployWebModel(spSite.RootWeb, model);
            }
        }

        protected void DeployCSOMModel(ModelNode model)
        {
            // deploy with CSOM
            var csomProvisionService = new StandardCSOMProvisionService();

            using (var context = new ClientContext(CSOMSiteUrl))
            {
                if (model.Value is SiteDefinition)
                    csomProvisionService.DeploySiteModel(context, model);

                if (model.Value is WebDefinition)
                    csomProvisionService.DeployWebModel(context, model);
            }
        }

        #endregion
    }
}
