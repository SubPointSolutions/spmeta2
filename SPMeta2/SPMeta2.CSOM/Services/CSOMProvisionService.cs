using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Utils;
using System.Reflection;

namespace SPMeta2.CSOM.Services
{
    public class CSOMProvisionService : ModelServiceBase
    {
        #region constructors

        public CSOMProvisionService()
        {
            RegisterModelHandlers();
            CheckSharePointRuntimeVersion();
        }

        private void CheckSharePointRuntimeVersion()
        {
            //var asm = typeof (Web).Assembly;
        }

        private void RegisterModelHandlers()
        {
            ModelHandlers.Clear();

            var handlerTypes = ReflectionUtils.GetTypesFromAssembly<CSOMModelHandlerBase>(Assembly.GetExecutingAssembly());

            foreach (var handlerType in handlerTypes)
            {
                var handlerInstance = Activator.CreateInstance(handlerType) as CSOMModelHandlerBase;

                if (handlerInstance != null)
                {
                    if (!ModelHandlers.ContainsKey(handlerInstance.TargetType))
                        ModelHandlers.Add(handlerInstance.TargetType, handlerInstance);
                }
            }
        }

        #endregion

        #region methods

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is CSOMModelHostBase))
                throw new ArgumentException("model host for CSOM needs to be inherited from CSOMModelHostBase");

            base.DeployModel(modelHost, model);
        }

        public override void RetractModel(ModelHostBase modelHost, ModelNode model)
        {
            if (!(modelHost is CSOMModelHostBase))
                throw new ArgumentException("model host for CSOM needs to be inherited from CSOMModelHostBase");

            base.RetractModel(modelHost, model);
        }

        #endregion
    }
}
