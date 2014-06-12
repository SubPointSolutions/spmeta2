using System;
using System.Security;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.Models;
using SPMeta2.Regression.CSOM;
using SPMeta2.Regression.Runners.Consts;

namespace SPMeta2.Regression.Runners.O365
{
    public class O365ProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        private static string GetEnvironmentVariable(string varName)
        {
            var result = Environment.GetEnvironmentVariable(varName);

            if (string.IsNullOrEmpty(result))
                result = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);

            return result;
        }

        public O365ProvisionRunner()
        {
            SiteUrl = GetEnvironmentVariable(EnvironmentConsts.O365_SiteUrl);
            UserName = GetEnvironmentVariable(EnvironmentConsts.O365_UserName);
            UserPassword = GetEnvironmentVariable(EnvironmentConsts.O365_Password);
        }

        #endregion

        #region properties

        public string UserName { get; set; }
        public string UserPassword { get; set; }

        public string SiteUrl { get; set; }
        public string WebUrl { get; set; }

        private readonly CSOMProvisionService _provisionService = new CSOMProvisionService();
        private readonly CSOMValidationService _validationService = new CSOMValidationService();


        #endregion

        #region methods

        public override void DeploySiteModel(ModelNode model)
        {
            WithO365Context(context =>
            {
                _provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);
                // _validationService.DeployModel(SiteModelHost.FromClientContext(context), model);
            });
        }

        public override void DeployWebModel(ModelNode model)
        {
            WithO365Context(context =>
            {
                _provisionService.DeployModel(WebModelHost.FromClientContext(context), model);
                // _validationService.DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        #endregion

        #region utils

        #region static

        private static SecureString GetSecurePasswordString(string password)
        {
            var securePassword = new SecureString();

            foreach (var s in password)
                securePassword.AppendChar(s);

            return securePassword;
        }

        #endregion

        public void WithO365Context(Action<ClientContext> action)
        {
            WithO365Context(SiteUrl, action);
        }

        public void WithO365Context(string siteUrl, Action<ClientContext> action)
        {
            WithO365Context(siteUrl, UserName, UserPassword, action);
        }

        private void WithO365Context(string siteUrl, string userName, string userPassword, Action<ClientContext> action)
        {
            using (var context = new ClientContext(siteUrl))
            {
                context.Credentials = new SharePointOnlineCredentials(userName, GetSecurePasswordString(userPassword));
                action(context);
            }
        }

        private void WithStaticO365SiteSandbox(string siteUrl, string userName, string userPassword, Action<ClientContext> action)
        {
            WithO365Context(siteUrl, userName, userPassword, action);
        }

        #endregion
    }
}
