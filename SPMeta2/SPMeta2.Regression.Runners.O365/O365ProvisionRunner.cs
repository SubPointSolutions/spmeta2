using System;
using System.Security;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.Models;
using SPMeta2.Regression.CSOM;
using SPMeta2.Regression.Runners.Consts;
using SPMeta2.Regression.Runners.Utils;
using Microsoft.SharePoint.WorkflowServices;
using Microsoft.SharePoint.Client.WorkflowServices;
using System.Collections.Generic;
using System.Diagnostics;

namespace SPMeta2.Regression.Runners.O365
{
    public class O365ProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        public O365ProvisionRunner()
        {
            Name = "O365";

            SiteUrls = new List<string>();
            WebUrls = new List<string>();

            LoadEnvironmentConfig();

            UserName = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.O365_UserName);
            UserPassword = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.O365_Password);
        }

        private void LoadEnvironmentConfig()
        {
            SiteUrls.Clear();
            SiteUrls.AddRange(RunnerEnvironment.GetEnvironmentVariables(EnvironmentConsts.O365_SiteUrls));

            WebUrls.Clear();
            WebUrls.AddRange(RunnerEnvironment.GetEnvironmentVariables(EnvironmentConsts.O365_WebUrls));
        }

        #endregion

        #region properties

        public List<string> SiteUrls { get; set; }
        public List<string> WebUrls { get; set; }

        public string UserName { get; set; }
        public string UserPassword { get; set; }

        private readonly CSOMProvisionService _provisionService = new CSOMProvisionService();
        private readonly CSOMValidationService _validationService = new CSOMValidationService();


        #endregion

        #region methods

        public override string ResolveFullTypeName(string typeName, string assemblyName)
        {
            var type = typeof(Field);
            var workflow = typeof(WorkflowDefinition);

            return base.ResolveFullTypeName(typeName, assemblyName);
        }

        public override void DeploySiteModel(ModelNode model)
        {
            foreach (var siteUrl in SiteUrls)
            {
                Trace.WriteLine(string.Format("[INF]    Running on site: [{0}]", siteUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithO365Context(siteUrl, context =>
                    {
                        _provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(SiteModelHost.FromClientContext(context), model);
                    });
                }
            }
        }

        public override void DeployWebModel(ModelNode model)
        {
            foreach (var webUrl in WebUrls)
            {
                Trace.WriteLine(string.Format("[INF]    Running on web: [{0}]", webUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithO365Context(webUrl, context =>
                    {
                        _provisionService.DeployModel(WebModelHost.FromClientContext(context), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(WebModelHost.FromClientContext(context), model);
                    });
                }
            }
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


        #endregion
    }
}
