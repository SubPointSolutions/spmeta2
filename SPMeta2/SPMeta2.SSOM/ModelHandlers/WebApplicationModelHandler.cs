using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using Microsoft.SharePoint.Administration;
using System.Security;
using SPMeta2.ModelHosts;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebApplicationModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(WebApplicationDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var webApplicationDefinition = model.WithAssertAndCast<WebApplicationDefinition>("model", value => value.RequireNotNull());

            DeployWebApplication(farmModelHost, farmModelHost.HostFarm, webApplicationDefinition);
        }

        protected virtual SPWebApplication LookupWebApplication(WebApplicationDefinition definition)
        {
            var webApps = SPWebService.ContentService.WebApplications;
            return FindWebApplication(definition, webApps);
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var webApplicationDefinition = model.WithAssertAndCast<WebApplicationDefinition>("model", value => value.RequireNotNull());

            if (modelHost is WebApplicationModelHost)
            {
                base.WithResolvingModelHost(modelHostContext);
                return;

            }

            var farmModelHost = modelHost.WithAssertAndCast<FarmModelHost>("modelHost", value => value.RequireNotNull());
            var existingWebApp = LookupWebApplication(webApplicationDefinition);

            if (existingWebApp == null)
            {
                throw new SPMeta2Exception(string.Format(
                    "Cannot find web aplication by definition:[]",
                    webApplicationDefinition));
            }

            var webAppModelHost = ModelHostBase.Inherit<WebApplicationModelHost>(farmModelHost, h =>
            {
                h.HostWebApplication = existingWebApp;
            });

            action(webAppModelHost);
        }

        private void DeployWebApplication(FarmModelHost farmModelHost,
            SPFarm farm,
            WebApplicationDefinition definition)
        {
            var existingWebApp = LookupWebApplication(definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = existingWebApp,
                ObjectType = typeof(SPWebApplication),
                ObjectDefinition = definition,
                ModelHost = farmModelHost
            });

            if (existingWebApp == null)
            {
                var webAppBuilder = new SPWebApplicationBuilder(farm);

                webAppBuilder.Port = definition.Port;
                webAppBuilder.ApplicationPoolId = definition.ApplicationPoolId;

                if (!string.IsNullOrEmpty(definition.ManagedAccount))
                {
                    webAppBuilder.IdentityType = IdentityType.SpecificUser;

                    var managedAccounts = new SPFarmManagedAccountCollection(SPFarm.Local);
                    var maccount = managedAccounts.FindOrCreateAccount(definition.ManagedAccount);

                    webAppBuilder.ManagedAccount = maccount;
                }
                else
                {
                    webAppBuilder.ApplicationPoolUsername = definition.ApplicationPoolUsername;

                    var password = new SecureString();

                    foreach (char c in definition.ApplicationPoolPassword.ToCharArray())
                        password.AppendChar(c);

                    webAppBuilder.ApplicationPoolPassword = password;
                }

                webAppBuilder.CreateNewDatabase = definition.CreateNewDatabase;

                webAppBuilder.DatabaseName = definition.DatabaseName;
                webAppBuilder.DatabaseServer = definition.DatabaseServer;

                webAppBuilder.UseNTLMExclusively = definition.UseNTLMExclusively;

                webAppBuilder.HostHeader = definition.HostHeader;
                webAppBuilder.AllowAnonymousAccess = definition.AllowAnonymousAccess;
                webAppBuilder.UseSecureSocketsLayer = definition.UseSecureSocketsLayer;

                var webApp = webAppBuilder.Create();
                webApp.Provision();

                ProcessWebApplicationProperties(existingWebApp, definition);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = webApp,
                    ObjectType = typeof(SPWebApplication),
                    ObjectDefinition = definition,
                    ModelHost = farmModelHost
                });

                webApp.Update();
            }
            else
            {
                ProcessWebApplicationProperties(existingWebApp, definition);

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = existingWebApp,
                    ObjectType = typeof(SPWebApplication),
                    ObjectDefinition = definition,
                    ModelHost = farmModelHost
                });

                existingWebApp.Update();
            }
        }

        protected virtual void ProcessAllowedInlineDownloadedMimeTypes(SPWebApplication webApp, WebApplicationDefinition definition)
        {
            if (definition.AllowedInlineDownloadedMimeTypes != null && definition.AllowedInlineDownloadedMimeTypes.Any())
            {
                // override if only ShouldOverrideAllowedInlineDownloadedMimeTypes is set to tru
                if (definition.ShouldOverrideAllowedInlineDownloadedMimeTypes == true)
                    webApp.AllowedInlineDownloadedMimeTypes.Clear();

                foreach (var value in definition.AllowedInlineDownloadedMimeTypes)
                    if (!webApp.AllowedInlineDownloadedMimeTypes.Contains(value))
                        webApp.AllowedInlineDownloadedMimeTypes.Add(value);
            }
        }

        protected virtual void ProcessWebApplicationProperties(SPWebApplication webApp, WebApplicationDefinition definition)
        {
            ProcessAllowedInlineDownloadedMimeTypes(webApp, definition);
        }

        private static SPWebApplication FindWebApplication(WebApplicationDefinition definition, SPWebApplicationCollection webApps)
        {
            var existingWebApp = webApps.FirstOrDefault(w =>
            {
                var webAppUri = w.GetResponseUri(SPUrlZone.Default);

                return webAppUri.Port == definition.Port;
            });
            return existingWebApp;
        }

        #endregion

    }
}
