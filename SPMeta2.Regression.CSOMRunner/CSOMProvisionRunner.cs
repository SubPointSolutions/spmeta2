using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.Models;
using SPMeta2.Regression.CSOM;
using SPMeta2.Regression.Runners;

namespace SPMeta2.Regression.CSOMRunner
{
    public static class EnvConsts
    {
        public const string O365_SiteUrl = "SPMeta2_O365_SiteUrl";
        public const string O365_UserName = "SPMeta2_O365_UserName";
        public const string O365_Password = "SPMeta2_O365_Password";
    }

    public class CSOMProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        private static string GetEnvironmentVariable(string varName)
        {
            var result = Environment.GetEnvironmentVariable(varName);

            if (string.IsNullOrEmpty(result))
                result = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);

            return result;
        }

        public CSOMProvisionRunner()
        {
            SiteUrl = GetEnvironmentVariable(EnvConsts.O365_SiteUrl);
            UserName = GetEnvironmentVariable(EnvConsts.O365_UserName);
            UserPassword = GetEnvironmentVariable(EnvConsts.O365_Password);
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
                _validationService.DeployModel(SiteModelHost.FromClientContext(context), model);
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
