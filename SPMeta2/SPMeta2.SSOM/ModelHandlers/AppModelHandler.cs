using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;

using Microsoft.SharePoint.Administration;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;
using System.IO;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class AppModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(AppDefinition); }
        }

        private const int MaxInstallAttempCount = 180;
        private const int WaitTimeInMillliseconds = 1000;

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var appModel = model.WithAssertAndCast<AppDefinition>("model", value => value.RequireNotNull());

            DeployApp(modelHost, webHost, appModel);
        }

        protected IList<SPAppInstance> FindExistingApps(WebModelHost webHost, AppDefinition appModel)
        {
            return webHost.HostWeb.GetAppInstancesByProductId(appModel.ProductId);
        }

        private void DeployApp(object modelHost, WebModelHost webHost, AppDefinition appModel)
        {
            var web = webHost.HostWeb;
            Guid appId = Guid.Empty;

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Creating memory stream on appModel.Content");


            using (var appPackage = new MemoryStream(appModel.Content))
            {
                var currentApplications = FindExistingApps(webHost, appModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = currentApplications.FirstOrDefault(),
                    ObjectType = typeof(SPAppInstance),
                    ObjectDefinition = appModel,
                    ModelHost = modelHost
                });

                if (currentApplications == null || currentApplications.Count == 0)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Cannot find application by productId. Loading and installing new instance.");

                    // install new
                    var newAppInstance = web.LoadAndInstallApp(appPackage);

                    if (newAppInstance != null && newAppInstance.Status == SPAppInstanceStatus.Initialized)
                    {
                        appId = newAppInstance.Id;

                        var count = 0;
                        SPAppInstance localInstance = null;

                        do
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                               "Waiting while app is being installed for [{0}] milliseconds.",
                               WaitTimeInMillliseconds);

                            Thread.Sleep(WaitTimeInMillliseconds);
                            localInstance = web.GetAppInstanceById(appId);

                            count++;
                        } while (localInstance != null &&
                                 localInstance.Status != SPAppInstanceStatus.Installed &&
                                 count < MaxInstallAttempCount);


                    }

                    newAppInstance = web.GetAppInstanceById(appId);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = newAppInstance,
                        ObjectType = typeof(SPAppInstance),
                        ObjectDefinition = appModel,
                        ModelHost = modelHost
                    });
                }
                else
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing application");

                    for (int i = 0; i < currentApplications.Count; i++)
                    {
                        var upApp = currentApplications[i];
                        var upVersion = new Version(upApp.App.VersionString);

                        var targetVersion = new Version(appModel.Version);

                        if (upVersion < targetVersion)
                            currentApplications[i].Upgrade(appPackage);
                    }

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = currentApplications.FirstOrDefault(),
                        ObjectType = typeof(SPAppInstance),
                        ObjectDefinition = appModel,
                        ModelHost = modelHost
                    });
                }
            }
        }

        #endregion
    }
}
