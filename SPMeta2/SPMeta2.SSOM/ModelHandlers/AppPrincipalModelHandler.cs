using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class AppPrincipalModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(AppPrincipalDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var appPrincipalModel = model.WithAssertAndCast<AppPrincipalDefinition>("model", value => value.RequireNotNull());

            DeployAppPrincipal(modelHost, webHost, appPrincipalModel);
        }

        protected virtual SPAppPrincipal FindExistingAppPrincipal(WebModelHost webHost,
            AppPrincipalDefinition appPrincipalModel)
        {
            var appPrincipalManager = SPAppPrincipalManager.GetManager(webHost.HostWeb);
            var appPrincipalProvider = SPAppPrincipalIdentityProvider.External;

            var appPrincipalName = SPAppPrincipalName.CreateFromAppPrincipalIdentifier(appPrincipalModel.AppId);
            return appPrincipalManager.LookupAppPrincipal(appPrincipalProvider, appPrincipalName);
        }

        private void DeployAppPrincipal(object modelHost, WebModelHost webHost, AppPrincipalDefinition appPrincipalModel)
        {
            var appPrincipalManager = SPAppPrincipalManager.GetManager(webHost.HostWeb);
            var principal = FindExistingAppPrincipal(webHost, appPrincipalModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = principal,
                ObjectType = typeof(SPAppPrincipal),
                ObjectDefinition = appPrincipalModel,
                ModelHost = modelHost
            });

            if (principal == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing app principal");

                var endpoints = new List<string>();
                endpoints.Add(appPrincipalModel.RedirectURI);

                var secureString = new SecureString();

                for (int i = 0; i < appPrincipalModel.AppSecret.Length; i++)
                {
                    char c = appPrincipalModel.AppSecret[i];
                    secureString.AppendChar(c);
                }
                secureString.MakeReadOnly();

                var now = DateTime.Now;

                var credential = SPAppPrincipalCredential.CreateFromSymmetricKey(secureString, now, now.AddYears(1));

                var externalAppPrincipalCreationParameters = new SPExternalAppPrincipalCreationParameters(appPrincipalModel.AppId, appPrincipalModel.Title, endpoints, credential)
                {
                    SkipExternalDirectoryRegistration = false
                };

                principal = appPrincipalManager.CreateAppPrincipal(externalAppPrincipalCreationParameters);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = principal,
                    ObjectType = typeof(SPAppPrincipal),
                    ObjectDefinition = appPrincipalModel,
                    ModelHost = modelHost
                });
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing app principal");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = principal,
                    ObjectType = typeof(SPAppPrincipal),
                    ObjectDefinition = appPrincipalModel,
                    ModelHost = modelHost
                });
            }
        }

        #endregion
    }
}
