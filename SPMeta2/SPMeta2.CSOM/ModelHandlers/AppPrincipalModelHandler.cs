using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class AppPrincipalModelHandler : CSOMModelHandlerBase
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
            TraceService.Error((int)LogEventId.ModelProvisionCoreCall, "AppPrincipal provision via CSOM is not supported by SharePoint. Throwing SPMeta2NotSupportedException");

            throw new SPMeta2NotSupportedException("AppPrincipal provision via CSOM is not supported by SharePoint.");

            var webHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var appPrincipalModel = model.WithAssertAndCast<AppPrincipalDefinition>("model", value => value.RequireNotNull());

            DeployAppPrincipal(modelHost, webHost, appPrincipalModel);
        }

        protected virtual AppPrincipal FindExistingAppPrincipal(WebModelHost webHost, AppPrincipalDefinition appPrincipalModel)
        {
            var context = webHost.HostWeb.Context;
            //var context = webHost.HostclientContext;

            var appPrincipalManager = AppPrincipalManager.GetManager(context, webHost.HostWeb);
            var appPrincipalProvider = AppPrincipalIdentityProvider.GetExternal(context);

            var appPrincipalName = AppPrincipalName.CreateFromAppPrincipalIdentifier(context, appPrincipalModel.AppId);
            var result = appPrincipalManager.LookupAppPrincipal(appPrincipalProvider, appPrincipalName);

            context.Load(result);
            context.ExecuteQueryWithTrace();

            return result;
        }

        private void DeployAppPrincipal(object modelHost, WebModelHost webHost, AppPrincipalDefinition appPrincipalModel)
        {
            var context = webHost.HostWeb.Context;

            var appPrincipalManager = AppPrincipalManager.GetManager(context, webHost.HostWeb);
            var principal = FindExistingAppPrincipal(webHost, appPrincipalModel);

            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = principal,
                ObjectType = typeof(AppPrincipal),
                ObjectDefinition = appPrincipalModel,
                ModelHost = modelHost
            });


            if (principal == null)
            {
                var endpoints = new List<string>();
                endpoints.Add(appPrincipalModel.RedirectURI);

                var now = DateTime.Now;

                var credential = AppPrincipalCredential.CreateFromSymmetricKey(context, appPrincipalModel.AppSecret, now, now.AddYears(1));

                var externalAppPrincipalCreationParameters = new ExternalAppPrincipalCreationParameters()
                {
                    AppIdentifier = appPrincipalModel.AppId,
                    DisplayName = appPrincipalModel.Title,
                    Credential = credential,
                    ApplicationEndpointAuthorities = endpoints
                };

                principal = appPrincipalManager.CreateAppPrincipal(externalAppPrincipalCreationParameters);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = principal,
                    ObjectType = typeof(AppPrincipal),
                    ObjectDefinition = appPrincipalModel,
                    ModelHost = modelHost
                });
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = principal,
                    ObjectType = typeof(AppPrincipal),
                    ObjectDefinition = appPrincipalModel,
                    ModelHost = modelHost
                });
            }
        }

        #endregion
    }
}
