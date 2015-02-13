using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;


namespace SPMeta2.Regression.CSOM.Base
{
    public class CSOMTestBase
    {
        #region properties

        public string UserName { get; set; }
        public string UserPassword { get; set; }

        public string SiteUrl { get; set; }
        public string WebUrl { get; set; }


        #endregion

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
    }
}
