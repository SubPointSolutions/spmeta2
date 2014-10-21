using System;
using System.Security;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing.Navigation;
using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.CSOM.ModelHandlers.Webparts;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using SPMeta2.Regression.CSOM.Standard.Validation.Fields;
using SPMeta2.Regression.CSOM.Validation.Webparts;
using SPMeta2.Regression.Runners.Consts;
using SPMeta2.Regression.Runners.Utils;
using SPMeta2.Regression.CSOM;
using Microsoft.SharePoint.WorkflowServices;
using Microsoft.SharePoint.Client.WorkflowServices;
using System.Collections.Generic;
using System.Diagnostics;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Runners.CSOM
{
    public class CSOMProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        public CSOMProvisionRunner()
        {
            Name = "CSOM";

            SiteUrls = new List<string>();
            WebUrls = new List<string>();

            LoadEnvironmentConfig();
            InitServices();

            UserName = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.CSOM_UserName);
            UserPassword = RunnerEnvironment.GetEnvironmentVariable(EnvironmentConsts.CSOM_Password);
        }

        private void InitServices()
        {
            _provisionService = new CSOMProvisionService();
            _validationService = new CSOMValidationService();

            var csomStandartAsm = typeof(TaxonomyFieldModelHandler).Assembly;

            foreach (var handlerType in ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(csomStandartAsm))
                _provisionService.RegisterModelHandler(Activator.CreateInstance(handlerType) as ModelHandlerBase);

            var csomtandartValidationAsm = typeof(ClientTaxonomyFieldDefinitionValidator).Assembly;

            foreach (var handlerType in ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(csomtandartValidationAsm))
                _validationService.RegisterModelHandler(Activator.CreateInstance(handlerType) as ModelHandlerBase);
        }

        private void LoadEnvironmentConfig()
        {
            SiteUrls.Clear();
            SiteUrls.AddRange(RunnerEnvironment.GetEnvironmentVariables(EnvironmentConsts.CSOM_SiteUrls));

            WebUrls.Clear();
            WebUrls.AddRange(RunnerEnvironment.GetEnvironmentVariables(EnvironmentConsts.CSOM_WebUrls));
        }

        #endregion

        #region properties

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Target site URLs
        /// </summary>
        public List<string> SiteUrls { get; set; }

        /// <summary>
        /// Target web URLs
        /// </summary>
        public List<string> WebUrls { get; set; }

        private CSOMProvisionService _provisionService;
        private CSOMValidationService _validationService;

        #endregion

        #region methods

        /// <summary>
        /// Resolves full name of the target type.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public override string ResolveFullTypeName(string typeName, string assemblyName)
        {
            var type = typeof(Field);
            var workflow = typeof(WorkflowDefinition);
            var store = typeof(TermStore);
            var publishing = typeof(WebNavigationSettings);

            return base.ResolveFullTypeName(typeName, assemblyName);
        }

        /// <summary>
        /// Deploys and validates target site model.
        /// </summary>
        /// <param name="model"></param>
        public override void DeploySiteModel(ModelNode model)
        {
            foreach (var siteUrl in SiteUrls)
            {
                Trace.WriteLine(string.Format("[INF]    Running on site: [{0}]", siteUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithCSOMContext(siteUrl, context =>
                    {
                        _provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(SiteModelHost.FromClientContext(context), model);
                    });
                }
            }
        }


        /// <summary>
        /// Deploys and validates target web model.
        /// </summary>
        /// <param name="model"></param>
        public override void DeployWebModel(ModelNode model)
        {
            foreach (var webUrl in WebUrls)
            {
                Trace.WriteLine(string.Format("[INF]    Running on web: [{0}]", webUrl));

                WithCSOMContext(webUrl, context =>
                {
                    for (var provisionGeneration = 0;
                        provisionGeneration < ProvisionGenerationCount;
                        provisionGeneration++)
                    {
                        _provisionService.DeployModel(WebModelHost.FromClientContext(context), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(WebModelHost.FromClientContext(context), model);
                    }
                });
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

        /// <summary>
        /// Invokes given action under CSOM client context.
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="action"></param>
        public void WithCSOMContext(string siteUrl, Action<ClientContext> action)
        {
            WithCSOMContext(siteUrl, UserName, UserPassword, action);
        }

        /// <summary>
        /// Invokes given action under CSOM client context.
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <param name="action"></param>
        private void WithCSOMContext(string siteUrl, string userName, string userPassword, Action<ClientContext> action)
        {
            using (var context = new ClientContext(siteUrl))
            {
                action(context);
            }
        }


        #endregion
    }
}
