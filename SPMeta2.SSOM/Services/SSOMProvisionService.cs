using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using System;
using System.Reflection;

namespace SPMeta2.SSOM.Services
{
    public class SSOMProvisionService : ModelServiceBase
    {
        #region contructors

        public SSOMProvisionService()
        {
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
            ModelHandlers.Clear();

            var handlerTypes = ReflectionUtils.GetTypesFromAssembly<SSOMModelHandlerBase>(Assembly.GetExecutingAssembly());

            foreach (var handlerType in handlerTypes)
            {
                var handlerInstance = Activator.CreateInstance(handlerType) as SSOMModelHandlerBase;

                if (handlerInstance != null)
                {
                    if (!ModelHandlers.ContainsKey(handlerInstance.TargetType))
                        ModelHandlers.Add(handlerInstance.TargetType, handlerInstance);
                }
            }
        }

        #endregion
    }
}
