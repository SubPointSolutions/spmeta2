using System;
using System.Security;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.Models;

using SPMeta2.Regression.Runners.Consts;
using SPMeta2.Regression.Runners.Utils;
using SPMeta2.Regression.CSOM;
using Microsoft.SharePoint.WorkflowServices;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace SPMeta2.Regression.Runners.CSOM
{
    public class CSOMProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        public CSOMProvisionRunner()
        {
            Name = "CSOM";

            SiteUrl = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.CSOM_SiteUrl);
            UserName = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.CSOM_UserName);
            UserPassword = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.CSOM_Password);
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

        public override string ResolveFullTypeName(string typeName, string assemblyName)
        {
            var type = typeof(Field);
            var workflow = typeof(WorkflowDefinition);

            return base.ResolveFullTypeName(typeName, assemblyName);
        }

        private ClientContext _lazyContext;

        public override void InitLazyRunnerConnection()
        {
            _lazyContext = new ClientContext(SiteUrl)
            {
                Credentials = new SharePointOnlineCredentials(UserName, GetSecurePasswordString(UserPassword))
            };
        }

        public override void DisposeLazyRunnerConnection()
        {
            if (_lazyContext != null)
            {
                _lazyContext.Dispose();
                _lazyContext = null;
            }
        }

        public override void DeploySiteModel(ModelNode model)
        {
            WithO365Context(context =>
            {
                _provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);

                if (EnableDefinitionValidation)
                    _validationService.DeployModel(SiteModelHost.FromClientContext(context), model);
            });
        }

        public override void DeployWebModel(ModelNode model)
        {
            WithO365Context(context =>
            {
                _provisionService.DeployModel(WebModelHost.FromClientContext(context), model);

                if (EnableDefinitionValidation)
                    _validationService.DeployModel(WebModelHost.FromClientContext(context), model);
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
            if (_lazyContext != null)
            {
                action(_lazyContext);
            }
            else
            {
                using (var context = new ClientContext(siteUrl))
                {
                    //context.Credentials = new SharePointOnlineCredentials(userName, GetSecurePasswordString(userPassword));

                    action(context);
                }
            }
        }


        #endregion
    }
}
