using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Services;
using SPMeta2.Utils;
using System.Reflection;

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
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "FindExistingApps() - finding app by productId: [{0}]", appModel.ProductId);

            var context = webHost.HostWeb.Context;
            var result = webHost.HostWeb.GetAppInstancesByProductId(appModel.ProductId);

            context.Load(result);
            context.ExecuteQueryWithTrace();

            return result;
        }

        private void DeployApp(object modelHost, WebModelHost webHost, AppDefinition appModel)
        {
            var web = webHost.HostWeb;
            var context = web.Context;

            var appId = Guid.Empty;

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
                    ObjectType = typeof(AppInstance),
                    ObjectDefinition = appModel,
                    ModelHost = modelHost
                });


                if (currentApplications == null || currentApplications.Count == 0)
                {
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Cannot find application by productId. Loading and installing new instance.");

                    // install new
                    var newAppInstance = web.LoadAndInstallApp(appPackage);



                    context.Load(newAppInstance);
                    context.ExecuteQueryWithTrace();

                    if (newAppInstance != null && newAppInstance.Status == AppInstanceStatus.Initialized)
                    {
                        appId = newAppInstance.Id;

                        var count = 0;
                        AppInstance localInstance = null;

                        do
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                                "Waiting while app is being installed for [{0}] milliseconds.",
                                WaitTimeInMillliseconds);

                            Thread.Sleep(WaitTimeInMillliseconds);
                            localInstance = web.GetAppInstanceById(appId);

                            context.Load(localInstance, l => l.Status);
                            context.ExecuteQueryWithTrace();

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
                    //we had check early
                    var currentApp = currentApplications.FirstOrDefault();

                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject,
                        string.Format("Processing existing application. Upgrading from [{0}] to [{1}]", currentApp.Title, appModel.Version));

                    var hasUpdate = false;

                    for (int i = 0; i < currentApplications.Count; i++)
                    {
                        var spApp = currentApplications[i];
                        //var spAppVersion = new Version(spApp.App.VersionString);

                        var definitionVersion = new Version(appModel.Version);

                        // always install
                        //if (definitionVersion > spAppVersion)
                        //{
                        TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Performing upgrade");

                        var updateAppInstance = currentApplications[i];
                        var updateAppId = updateAppInstance.Id;

                        var count = 0;
                        AppInstance localUpdateAppInstance = null;

                        try
                        {
                            updateAppInstance.Upgrade(appPackage);
                            context.ExecuteQueryWithTrace();
                        }
                        catch (Exception upgradeException)
                        {
                            // handling early version upgrades
                            // Microsoft.SharePoint.Client.ServerException]	
                            // {"An App Instance can only be upgraded to a newer version of the same product. The upgrade request was for product 1.0.0.3 version e81b6820-5d57-4d17-a098-5f4317f6c400 to product 1.0.0.0 version e81b6820-5d57-4d17-a098-5f4317f6c400."}

                            if (IsAppUpgradeException(upgradeException))
                            {
                                // fascinating
                                // jumping to the happy end
                                goto AppShouldNotBeUpdated;
                            }

                            throw;
                        }

                        do
                        {
                            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall,
                                "Waiting while app is being installed for [{0}] milliseconds.",
                                WaitTimeInMillliseconds);

                            Thread.Sleep(WaitTimeInMillliseconds);
                            localUpdateAppInstance = web.GetAppInstanceById(updateAppId);

                            context.Load(localUpdateAppInstance, l => l.Status);
                            context.ExecuteQueryWithTrace();

                            count++;
                        } while (localUpdateAppInstance != null &&
                                 localUpdateAppInstance.Status != AppInstanceStatus.Installed &&
                                 count < MaxInstallAttempCount);

                        hasUpdate = true;

                        //}
                        //else
                        //{
                        //    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Skipping upgrade due to the lower version");
                        //}
                    }

                    if (hasUpdate)
                    {
                        // refreshing the app collection after update
                        // the .App.VersionString property will be refreshed
                        currentApplications = FindExistingApps(webHost, appModel);
                    }

                AppShouldNotBeUpdated:

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

        /// <summary>
        /// Detects is there was an application upgrade exception to the early app version.
        /// </summary>
        /// <param name="upgradeException"></param>
        /// <returns></returns>
        protected virtual bool IsAppUpgradeException(Exception upgradeException)
        {
            if (upgradeException == null)
                return false;

            // Microsoft.SharePoint.Client.ServerException]	
            // {"An App Instance can only be upgraded to a newer version of the same product. The upgrade request was for product 1.0.0.3 version e81b6820-5d57-4d17-a098-5f4317f6c400 to product 1.0.0.0 version e81b6820-5d57-4d17-a098-5f4317f6c400."}


            if (upgradeException is ServerException)
            {
                // .net 4 hack to get HResult
                var hResultProp = upgradeException.GetType()
                                              .GetProperty("HResult",
                                                BindingFlags.NonPublic
                                                | BindingFlags.Public
                                                | BindingFlags.Instance
                                                | BindingFlags.Static);


                if (hResultProp != null)
                {
                    var hResultValue = hResultProp.GetValue(upgradeException, null);
                    if (hResultValue is int)
                    {
                        return (int)hResultValue == -2146233088;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
