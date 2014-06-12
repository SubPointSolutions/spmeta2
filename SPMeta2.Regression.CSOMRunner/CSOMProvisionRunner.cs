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
    public class CSOMProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        public CSOMProvisionRunner()
        {
           
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
