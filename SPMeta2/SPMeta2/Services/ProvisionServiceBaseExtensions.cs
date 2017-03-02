using SPMeta2.Services.Impl;
using SPMeta2.Services.Impl.Validation;
using SPMeta2.Utils;
using System;
using System.Linq;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Common;
using SPMeta2.Exceptions;

namespace SPMeta2.Services
{
    public static class ProvisionServiceBaseExtensions
    {
        #region incremental provision

        public static ProvisionServiceBase SetIncrementalMode(this ProvisionServiceBase service)
        {
            return SetIncrementalMode(service, null);
        }

        public static ProvisionServiceBase SetIncrementalMode(this ProvisionServiceBase service, IncrementalProvisionConfig config)
        {
            if (config == null)
                config = IncrementalProvisionConfig.Default;

            if (config.CustomModelTreeTraverseServiceType != null)
                service.CustomModelTreeTraverseServiceType = config.CustomModelTreeTraverseServiceType;
            else
                service.CustomModelTreeTraverseServiceType = typeof(DefaultIncrementalModelTreeTraverseService);

            var typedModelService = service.ModelTraverseService as DefaultIncrementalModelTreeTraverseService;
            typedModelService.PreviousModelHash = config.PreviousModelHash ?? new ModelHash();

            return service;
        }

        public static ProvisionServiceBase SetIncrementalModelHash(this ProvisionServiceBase service, ModelHash modelHash)
        {
            var typedModelService = GetIncrementalModelTraverseService(service);
            typedModelService.PreviousModelHash = modelHash;

            return service;
        }

        public static ModelHash GetIncrementalModelHash(this ProvisionServiceBase service)
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
    }
}