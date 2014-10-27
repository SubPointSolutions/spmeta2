using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class AppModelHandler : CSOMModelHandlerBase
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

        protected ClientObjectList<AppInstance> FindExistingApps(WebModelHost webHost, AppDefinition appModel)
        {
            var context = webHost.HostWeb.Context;
            var result = webHost.HostWeb.GetAppInstancesByProductId(appModel.ProductId);

            context.Load(result);
            context.ExecuteQuery();

            return result;
        }

        private void DeployApp(object modelHost, WebModelHost webHost, AppDefinition appModel)
        {
            var web = webHost.HostWeb;
            Guid appId = Guid.Empty;

            using (var appPackage = new MemoryStream(appModel.Content))
            {
                var currentApplications = FindExistingApps(webHost, appModel);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = currentApplications.FirstOrDefault(),
                    ObjectType = typeof(AppInstance),
                    ObjectDefinition = appModel,
                    ModelHost = modelHost
                });

                if (currentApplications == null || currentApplications.Count == 0)
                {
                    // install new
                    var newAppInstance = web.LoadAndInstallApp(appPackage);

                    var context = web.Context;

                    context.Load(newAppInstance);
                    context.ExecuteQuery();

                    if (newAppInstance != null && newAppInstance.Status == AppInstanceStatus.Initialized)
                    {
                        appId = newAppInstance.Id;

                        var count = 0;
                        AppInstance localInstance = null;

                        do
                        {
                            Thread.Sleep(WaitTimeInMillliseconds);
                            localInstance = web.GetAppInstanceById(appId);

                            count++;
                        } while (localInstance != null &&
                                 localInstance.Status != AppInstanceStatus.Installed &&
                                 count < MaxInstallAttempCount);


                    }

                    newAppInstance = web.GetAppInstanceById(appId);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = newAppInstance,
                        ObjectType = typeof(AppInstance),
                        ObjectDefinition = appModel,
                        ModelHost = modelHost
                    });
                }
                else
                {
                    // R&D for update
                    // upApp.App is not available

                    //for (int i = 0; i < currentApplications.Count; i++)
                    //{
                    //    var upApp = currentApplications[i];
                    //    var upVersion = new Version(upApp.App.VersionString);

                    //    var targetVersion = new Version(appModel.Version);

                    //    if (upVersion < targetVersion)
                    //        currentApplications[i].Upgrade(appPackage);
                    //}

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = currentApplications.FirstOrDefault(),
                        ObjectType = typeof(AppInstance),
                        ObjectDefinition = appModel,
                        ModelHost = modelHost
                    });
                }
            }
        }

        #endregion
    }
}
