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

        private void DeployWebApplication(FarmModelHost farmModelHost, SPFarm farm, WebApplicationDefinition webApplicationDefinition)
        {
            var webApps = SPWebService.ContentService.WebApplications;

            var webAppBuilder = new SPWebApplicationBuilder(farm);

            webAppBuilder.Port = webApplicationDefinition.Port;
            webAppBuilder.ApplicationPoolId = webApplicationDefinition.ApplicationPoolId;

            if (!string.IsNullOrEmpty(webApplicationDefinition.ManagedAccount))
            {
                webAppBuilder.IdentityType = IdentityType.SpecificUser;

                var managedAccounts = new SPFarmManagedAccountCollection(SPFarm.Local);
                var maccount = managedAccounts.FindOrCreateAccount(webApplicationDefinition.ManagedAccount);

                webAppBuilder.ManagedAccount = maccount;
            }
            else
            {
                webAppBuilder.ApplicationPoolUsername = webApplicationDefinition.ApplicationPoolUsername;

                var password = new SecureString();

                foreach (char c in webApplicationDefinition.ApplicationPoolPassword.ToCharArray())
                    password.AppendChar(c);

                webAppBuilder.ApplicationPoolPassword = password;
            }

            webAppBuilder.CreateNewDatabase = webApplicationDefinition.CreateNewDatabase;

            webAppBuilder.DatabaseName = webApplicationDefinition.DatabaseName;
            webAppBuilder.DatabaseServer = webApplicationDefinition.DatabaseServer;

            webAppBuilder.UseNTLMExclusively = webApplicationDefinition.UseNTLMExclusively;

            webAppBuilder.HostHeader = webApplicationDefinition.HostHeader;
            webAppBuilder.AllowAnonymousAccess = webApplicationDefinition.AllowAnonymousAccess;
            webAppBuilder.UseSecureSocketsLayer = webApplicationDefinition.UseSecureSocketsLayer;

            var webApp = webAppBuilder.Create();
            webApp.Provision();
        }

        #endregion

    }
}
