using SPMeta2.Services.Impl;
using SPMeta2.Services.Impl.Validation;
using SPMeta2.Utils;
using System;
using System.Linq;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Common;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Extensions;

namespace SPMeta2.Services
{
    public static class IncrementalProvisionServiceBaseExtensions
    {
        #region default provision

        public static ProvisionServiceBase ResetProvisionMode(this ProvisionServiceBase service)
        {
            return SetDefaultProvisionMode(service);
        }

        public static ProvisionServiceBase SetDefaultProvisionMode(this ProvisionServiceBase service)
        {
            service.CustomModelTreeTraverseServiceType = null;
            service.ModelTraverseService = null;

            return service;
        }

        #endregion

        #region incremental provision

        public static ProvisionServiceBase SetIncrementalProvisionMode(this ProvisionServiceBase service)
        {
            return SetIncrementalProvisionMode(service, null);
        }

        public static ProvisionServiceBase SetIncrementalProvisionMode(this ProvisionServiceBase service, IncrementalProvisionConfig config)
        {
            service.ModelTraverseService = null;

            if (config == null)
                config = IncrementalProvisionConfig.Default;

            if (config.CustomModelTreeTraverseServiceType != null)
                service.CustomModelTreeTraverseServiceType = config.CustomModelTreeTraverseServiceType;
            else
                service.CustomModelTreeTraverseServiceType = typeof(DefaultIncrementalModelTreeTraverseService);

            var typedModelService = service.ModelTraverseService as DefaultIncrementalModelTreeTraverseService;
            typedModelService.PreviousModelHash = config.PreviousModelHash ?? new ModelHash();

            typedModelService.Configuration = config;

            return service;
        }

        public static ProvisionServiceBase SetIncrementalProvisionModelHash(this ProvisionServiceBase service, ModelHash modelHash)
        {
            var typedModelService = GetIncrementalModelTraverseService(service);
            typedModelService.PreviousModelHash = modelHash;

            return service;
        }

        public static ModelHash GetIncrementalProvisionModelHash(this ProvisionServiceBase service)
        {
            var typedModelService = GetIncrementalModelTraverseService(service);
            return typedModelService.CurrentModelHash;
        }

        #endregion

        #region utils

        private static DefaultIncrementalModelTreeTraverseService GetIncrementalModelTraverseService(this ProvisionServiceBase service)
        {
            if (!(service.ModelTraverseService is DefaultIncrementalModelTreeTraverseService))
                throw new SPMeta2Exception(
                    string.Format("provision server isn't set in incremenatl mode. Call .SetIncrementalMode() before calling the current method"));

            var typedModelService = service.ModelTraverseService as DefaultIncrementalModelTreeTraverseService;

            return typedModelService;
        }

        #endregion

        #region deployment helpers

        /// <summary>
        /// A shortcut for incremental provision
        /// Sets incremental provision mode with AutoDetectSharePointPersistenceStorage = true
        /// Once done, reverts back to default provision mode
        /// </summary>
        /// <param name="provisionService"></param>
        /// <param name="modelHost"></param>
        /// <param name="model"></param>
        /// <param name="incrementalModelId"></param>
        public static void DeployModelIncrementally(this ProvisionServiceBase provisionService,
            ModelHostBase modelHost,
            ModelNode model,
            string incrementalModelId)
        {
            DeployModelIncrementally(provisionService, modelHost, model, incrementalModelId, null);
        }

        /// <summary>
        /// A shortcut for incremental provision
        /// Sets incremental provision mode with IncrementalProvisionConfig.AutoDetectSharePointPersistenceStorage = true
        /// Once done, reverts back to default provision mode
        /// Callback on IncrementalProvisionConfig makes it easy to configure IncrementalProvisionConfig instance
        /// </summary>
        /// <param name="provisionService"></param>
        /// <param name="modelHost"></param>
        /// <param name="model"></param>
        /// <param name="incrementalModelId"></param>
        /// <param name="config"></param>
        public static void DeployModelIncrementally(this ProvisionServiceBase provisionService,
            ModelHostBase modelHost,
            ModelNode model,
            string incrementalModelId,
            Action<IncrementalProvisionConfig> config)
        {
            try
            {
                var incrementalProvisionConfig = new IncrementalProvisionConfig
                {
                    AutoDetectSharePointPersistenceStorage = true
                };

                if (config != null)
                    config(incrementalProvisionConfig);

                provisionService.SetIncrementalProvisionMode(incrementalProvisionConfig);
                model.InternalSetIncrementalProvisionModelId(incrementalModelId);

                provisionService.DeployModel(modelHost, model);
            }
            finally
            {
                provisionService.SetDefaultProvisionMode();
            }
        }

        #endregion
    }
}