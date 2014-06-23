using System;
using System.Diagnostics;
using System.IO;
using System.Security;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Profiles;

namespace SPMeta2.Regression.Base
{
    //[TestClass]
    public class SPWebApplicationSandboxTest
    {
        #region contructors

        public SPWebApplicationSandboxTest()
        {
           
        }

        public string SiteUrl { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        #endregion

        #region properties

        public WebApplicationSandboxProfile Profile { get; set; }

        #endregion

        #region methods

        protected virtual void InitTestSettings()
        {
            Profile = ProfileHelper.LoadCurrentProfile<WebApplicationSandboxProfile>();
        }

        protected virtual void CleanupResources()
        {
            CleanupWebs();
            CleanupSites();
        }

        protected virtual void CleanupSites()
        {

        }

        protected virtual void CleanupWebs()
        {

        }



        protected virtual string SiteCollectionTemplateName
        {
            get { return "STS#0"; }
        }

        protected virtual string SiteOwnerName
        {
            get { return string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName); }
        }

        public virtual void WithSPSiteSandbox(Action<SPSite, SPWeb> action)
        {
            var siteTitle = string.Format("test-{0}", Guid.NewGuid().ToString("N"));

            WithSPSiteSandbox(siteTitle, action, SiteCollectionTemplateName, SiteOwnerName, true);
        }

        //public virtual void WithSPSiteSandbox(Action<SPSite, SPWeb> action, bool deleteSite)
        //{
        //    WithSPSiteSandbox(SiteCollectionTemplateName, action, deleteSite);
        //}

        public virtual void WithSPSiteSandbox(string siteTitle, Action<SPSite, SPWeb> action, bool deleteSite)
        {
            WithSPSiteSandbox(siteTitle, action, SiteCollectionTemplateName, SiteOwnerName, deleteSite);
        }

        public virtual void WithStaticSPSiteSandbox(string siteTitle, Action<SPSite, SPWeb> action)
        {
            WithSPSiteSandbox(siteTitle, action, SiteCollectionTemplateName, SiteOwnerName, false);
        }

        public virtual void WithSPSiteSandbox(string siteTitle,
                                              Action<SPSite, SPWeb> action,
                                              string siteTemplate,
                                              string ownerAccount,
                                              bool deleteSiteCollection)
        {


            using (var site = EnsureSiteCollection(Profile.WebApplicationUrl, Profile.ManagedPath, siteTitle, siteTemplate, ownerAccount))
            {
                try
                {
                    using (var web = site.OpenWeb())
                    {
                        action(site, web);
                    }
                }
                finally
                {
                    if (deleteSiteCollection)
                        site.Delete();
                }
            }
        }

        protected virtual void EnsureManagedPath(string webApplicationUrl, string managedPath)
        {
            var webApp = SPWebApplication.Lookup(new Uri(webApplicationUrl));

            if (!webApp.Prefixes.Contains(managedPath))
                webApp.Prefixes.Add(managedPath, SPPrefixType.WildcardInclusion);
        }

        private SPSite EnsureSiteCollection(string webApplicationUrl,
                                            string managedPath,
                                                    string title,
                                                    string template,
                                                    string ownerLogin)
        {
            try
            {
                EnsureWebApplication(webApplicationUrl);
                EnsureManagedPath(webApplicationUrl, managedPath);

                var siteUrl = string.Format("{0}/{1}/{2}", Profile.WebApplicationUrl, managedPath, title);
                var site = new SPSite(siteUrl);

                return site;
            }
            catch (FileNotFoundException ensureSiteCollectionException)
            {
                Trace.WriteLine(string.Format("Exception:[{0}]", ensureSiteCollectionException));
                return CreateSiteCollection(managedPath, title, template, ownerLogin);
            }
        }

        protected virtual SPSite CreateSiteCollection(string managedPath,
                                                    string title,
                                                    string template,
                                                    string ownerLogin)
        {
            var webApplication = SPWebApplication.Lookup(new Uri(Profile.WebApplicationUrl));

            return webApplication.Sites.Add(string.Format("/{0}/{1}", managedPath, title),
                                    title,
                                    string.Empty,
                // might be issues as SP might not have you locale, huh
                //(UInt32)System.Globalization.CultureInfo.CurrentCulture.LCID,
                                    0,
                                    template,
                                    ownerLogin,
                                    "owner name",
                                    "owner@owner.com");
        }

        protected virtual void EnsureWebApplication(string webApplicatoinUrl)
        {
            var webApplication = SPWebApplication.Lookup(new Uri(webApplicatoinUrl));

            if (webApplication == null) throw new ArgumentException(
                string.Format("Can't lookup web application at url: {0}", webApplicatoinUrl));
        }

        #endregion

        private static SecureString GetSecurePasswordString(string password)
        {
            var securePassword = new SecureString();

            foreach (var s in password)
                securePassword.AppendChar(s);

            return securePassword;
        }

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
