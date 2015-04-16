using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System;
using System.Reflection;

namespace SPMeta2.SSOM.Services
{
    public class SSOMProvisionService : ModelServiceBase
    {
        #region constructors

        public SSOMProvisionService()
        {
            ServiceContainer.Instance.RegisterService(typeof(SSOMTokenReplacementService), new SSOMTokenReplacementService());

            RegisterModelHandlers();

            //CheckSharePointRuntimeVersion();
        }

        #endregion

        #region properties

        //private static Version MinimalSPRuntimeVersion = new Version("15.0.4569.1000");

        #endregion

        #region methods

        //private void CheckSharePointRuntimeVersion()
        //{
        //    var spRuntimeVersion = SPFarm.Local.BuildVersion;

        //    if (spRuntimeVersion < MinimalSPRuntimeVersion)
        //    {
        //        // TODO
        //    }
        //}

        private void RegisterModelHandlers()
        {
            RegisterModelHandlers(typeof(FieldModelHandler).Assembly);
        }

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is SSOMModelHostBase))
                throw new ArgumentException("modelHost for SSOM needs to be inherited from SSOMModelHostBase.");

            base.DeployModel(modelHost, model);
        }

        public override void RetractModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is SSOMModelHostBase))
                throw new ArgumentException("model host for SSOM needs to be inherited from SSOMModelHostBase.");

            base.RetractModel(modelHost, model);
        }

        #endregion
    }

    public static class SSOMProvisionServiceExtensions
    {
        public static void DeployFarmModel(this SSOMProvisionService modelHost, SPFarm farm, ModelNode model)
        {
            modelHost.DeployModel(FarmModelHost.FromFarm(farm), model);
        }

        public static void DeployWebApplicationModel(this SSOMProvisionService modelHost, SPWebApplication webApplication, ModelNode model)
        {
            modelHost.DeployModel(WebApplicationModelHost.FromWebApplication(webApplication), model);
        }

        public static void DeploySiteModel(this SSOMProvisionService modelHost, SPSite site, ModelNode model)
        {
            modelHost.DeployModel(SiteModelHost.FromSite(site), model);
        }

        public static void DeployWebModel(this SSOMProvisionService modelHost, SPWeb web, ModelNode model)
        {
            modelHost.DeployModel(WebModelHost.FromWeb(web), model);
        }
    }
}
