using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Utils;
using System.Reflection;
using System.Diagnostics;
using SPMeta2.CSOM.Services.Impl;
using SPMeta2.Exceptions;
using SPMeta2.Common;

namespace SPMeta2.CSOM.Services
{
    public class CSOMProvisionService : ProvisionServiceBase
    {
        #region constructors

        public CSOMProvisionService()
        {
            ServiceContainer.Instance.RegisterService(typeof(CSOMTokenReplacementService), new CSOMTokenReplacementService());
            ServiceContainer.Instance.RegisterService(typeof(CSOMLocalizationService), new CSOMLocalizationService());

            // default sharepoint persistence storage impl
            ServiceContainer.Instance.RegisterService(typeof(SharePointPersistenceStorageServiceBase), new DefaultCSOMWebPropertyBagStorage());

            // Align CSOM throttling setting with MS recommendations, open up API #849
            // https://github.com/SubPointSolutions/spmeta2/issues/849

            // register an instance of ClientRuntimeContextServiceBase -> DefaultClientRuntimeContextService
            ServiceContainer.Instance.RegisterService(typeof(ClientRuntimeContextServiceBase), new DefaultClientRuntimeContextService());

            PreDeploymentServices.Add(new RequireCSOMRuntimeVersionDeploymentService());

            RegisterModelHandlers();
        }

        private void RegisterModelHandlers()
        {
            RegisterModelHandlers(typeof(FieldModelHandler).Assembly);
        }

        #endregion

        #region methods

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is CSOMModelHostBase))
                throw new ArgumentException("model host for CSOM needs to be inherited from CSOMModelHostBase");

            var clientContext = (modelHost as CSOMModelHostBase).HostClientContext;

            PreloadProperties(clientContext);

            // TODO, check clientContext.ServerLibraryVersion to make sure it's >= SP2013 SP

            base.DeployModel(modelHost, model);
        }

        private static void PreloadProperties(ClientContext clientContext)
        {
            var needQuery = false;

            if (!clientContext.Site.IsPropertyAvailable("ServerRelativeUrl"))
            {
                clientContext.Load(clientContext.Site, s => s.ServerRelativeUrl);
                needQuery = true;
            }

            if (!clientContext.Web.IsPropertyAvailable("ServerRelativeUrl"))
            {
                clientContext.Load(clientContext.Web, w => w.ServerRelativeUrl);
                needQuery = true;
            }

            if (needQuery)
            {
                clientContext.ExecuteQueryWithTrace();
            }
        }

        public override void RetractModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is CSOMModelHostBase))
                throw new ArgumentException("model host for CSOM needs to be inherited from CSOMModelHostBase");

            base.RetractModel(modelHost, model);
        }

        #endregion
    }

    public static class CSOMProvisionServiceExtensions
    {
        public static void DeploySiteModel(this CSOMProvisionService modelHost, ClientContext context, ModelNode model)
        {
            modelHost.DeployModel(new SiteModelHost(context), model);
        }

        public static void DeployWebModel(this CSOMProvisionService modelHost, ClientContext context, ModelNode model)
        {
            modelHost.DeployModel(new WebModelHost(context), model);
        }

        public static void DeployListModel(this CSOMProvisionService modelHost, ClientContext context, List list, ModelNode model)
        {
            var listHost = ModelHostBase.Inherit<ListModelHost>(WebModelHost.FromClientContext(context), h =>
            {
                h.HostList = list;
            });

            modelHost.DeployModel(listHost, model);
        }
    }

    public static class SSOMProvisionServiceIncrementalExtensions
    {
        /// <summary>
        /// A shortcut for incremental provision
        /// Sets incremental provision mode with AutoDetectSharePointPersistenceStorage = true
        /// Once done, reverts back to default provision mode
        /// </summary>
        public static void DeploySiteModelIncrementally(this CSOMProvisionService modelHost,
            ClientContext context,
            ModelNode model,
            string incrementalModelId)
        {
            DeploySiteModelIncrementally(modelHost, context, model, incrementalModelId, null);
        }

        /// <summary>
        /// A shortcut for incremental provision
        /// Sets incremental provision mode with AutoDetectSharePointPersistenceStorage = true
        /// Once done, reverts back to default provision mode
        /// </summary>
        public static void DeploySiteModelIncrementally(this CSOMProvisionService modelHost,
            ClientContext context,
            ModelNode model,
            string incrementalModelId,
            Action<IncrementalProvisionConfig> config)
        {
            modelHost.DeployModelIncrementally(new SiteModelHost(context), model, incrementalModelId, config);
        }

        /// <summary>
        /// A shortcut for incremental provision
        /// Sets incremental provision mode with AutoDetectSharePointPersistenceStorage = true
        /// Once done, reverts back to default provision mode
        /// </summary>
        public static void DeployWebModelIncrementally(this CSOMProvisionService modelHost,
            ClientContext context,
            ModelNode model,
            string incrementalModelId)
        {
            DeployWebModelIncrementally(modelHost, context, model, incrementalModelId, null);
        }

        /// <summary>
        /// A shortcut for incremental provision
        /// Sets incremental provision mode with IncrementalProvisionConfig.AutoDetectSharePointPersistenceStorage = true
        /// Once done, reverts back to default provision mode
        /// Callback on IncrementalProvisionConfig makes it easy to configure IncrementalProvisionConfig instance
        public static void DeployWebModelIncrementally(this CSOMProvisionService modelHost,
            ClientContext context,
            ModelNode model,
            string incrementalModelId,
            Action<IncrementalProvisionConfig> config)
        {
            modelHost.DeployModelIncrementally(new WebModelHost(context), model, incrementalModelId, config);
        }
    }
}
