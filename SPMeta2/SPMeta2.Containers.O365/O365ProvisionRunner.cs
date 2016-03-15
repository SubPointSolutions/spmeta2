﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing.Navigation;
using Microsoft.SharePoint.Client.Search.Portability;
using Microsoft.SharePoint.Client.Taxonomy;
using Microsoft.SharePoint.Client.WorkflowServices;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Exceptions;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Utils;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.ModelHandlers.Fields;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using SPMeta2.Regression.CSOM;
using SPMeta2.Regression.CSOM.Standard.Validation.Fields;
using SPMeta2.Utils;
using SPMeta2.CSOM.Standard.Services;

namespace SPMeta2.Containers.O365
{
    /// <summary>
    /// O365 container runner implementation.
    /// </summary>
    public class O365ProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        public O365ProvisionRunner()
        {
            Name = "O365";

            SiteUrls = new List<string>();
            WebUrls = new List<string>();

            LoadEnvironmentConfig();
            InitServices();

            UserName = RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.O365_UserName);
            UserPassword = RunnerEnvironmentUtils.GetEnvironmentVariable(EnvironmentConsts.O365_Password);
        }

        private void InitServices()
        {
            _provisionService = new StandardCSOMProvisionService();
            _validationService = new CSOMValidationService();

            var csomStandartAsm = typeof(TaxonomyFieldModelHandler).Assembly;

            foreach (var handlerType in ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(csomStandartAsm))
                _provisionService.RegisterModelHandler(Activator.CreateInstance(handlerType) as ModelHandlerBase);

            var csomtandartValidationAsm = typeof(ClientTaxonomyFieldDefinitionValidator).Assembly;

            foreach (var handlerType in ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(csomtandartValidationAsm))
                _validationService.RegisterModelHandler(Activator.CreateInstance(handlerType) as ModelHandlerBase);

            _provisionService.OnModelNodeProcessing += (sender, args) =>
            {
                Trace.WriteLine(
                    string.Format("Processing: [{0}/{1}] - [{2:0} %] - [{3}] [{4}]",
                    new object[] {
                                  args.ProcessedModelNodeCount,
                                  args.TotalModelNodeCount,
                                  100d * (double)args.ProcessedModelNodeCount / (double)args.TotalModelNodeCount,
                                  args.CurrentNode.Value.GetType().Name,
                                  args.CurrentNode.Value
                                  }));
            };

            _provisionService.OnModelNodeProcessed += (sender, args) =>
            {
                Trace.WriteLine(
                   string.Format("Processed: [{0}/{1}] - [{2:0} %] - [{3}] [{4}]",
                   new object[] {
                                  args.ProcessedModelNodeCount,
                                  args.TotalModelNodeCount,
                                  100d * (double)args.ProcessedModelNodeCount / (double)args.TotalModelNodeCount,
                                  args.CurrentNode.Value.GetType().Name,
                                  args.CurrentNode.Value
                                  }));
            };
        }

        private List<string> RandomBalancedUrls = new List<string>();

        private void LoadEnvironmentConfig()
        {
            SiteUrls.Clear();
            SiteUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.O365_SiteUrls));

            WebUrls.Clear();
            WebUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.O365_WebUrls));

            WebUrls.Clear();
            WebUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.O365_WebUrls));

            RandomBalancedUrls.Clear();
            RandomBalancedUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.O365_RandomBalancedUrls));
        }

        #endregion

        #region properties

        /// <summary>
        /// Target site URLs
        /// </summary>
        public List<string> SiteUrls { get; set; }

        /// <summary>
        /// Target web URLs
        /// </summary>
        public List<string> WebUrls { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string UserPassword { get; set; }

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
            var search = typeof(SearchConfigurationPortability);

            return base.ResolveFullTypeName(typeName, assemblyName);
        }


        public override void DeployFarmModel(ModelNode model)
        {
            throw new SPMeta2UnsupportedCSOMRunnerException();
        }

        public override void DeployWebApplicationModel(ModelNode model)
        {
            throw new SPMeta2UnsupportedCSOMRunnerException();
        }

        private Random rnd = new Random();

        /// <summary>
        /// Deploys and validates target site model.
        /// </summary>
        /// <param name="model"></param>
        public override void DeploySiteModel(ModelNode model)
        {
            if (RandomBalancedUrls.Count > 0)
            {
                var url = RandomBalancedUrls[rnd.Next(0, RandomBalancedUrls.Count)];
                SiteOnUrl(model, url);
            }
            else
            {
                foreach (var siteUrl in SiteUrls)
                {
                    SiteOnUrl(model, siteUrl);
                }
            }
        }

        private void SiteOnUrl(ModelNode model, string siteUrl)
        {
            Trace.WriteLine(string.Format("[INF]    Running on site: [{0}]", siteUrl));

            for (var provisionGeneration = 0;
                provisionGeneration < ProvisionGenerationCount;
                provisionGeneration++)
            {
                WithO365Context(siteUrl, context =>
                {
                    if (EnableDefinitionProvision)
                        _provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);

                    if (EnableDefinitionValidation)
                        _validationService.DeployModel(SiteModelHost.FromClientContext(context), model);
                });
            }
        }

        public override void DeployListModel(ModelNode model)
        {

            if (RandomBalancedUrls.Count > 0)
            {
                var url = RandomBalancedUrls[rnd.Next(0, RandomBalancedUrls.Count)];
                WebOnUrl(model, url);
            }
            else
            {

                foreach (var webUrl in WebUrls)
                {
                    WebOnUrl(model, webUrl);
                }
            }
        }

        private void WebOnUrl(ModelNode model, string webUrl)
        {
            Trace.WriteLine(string.Format("[INF]    Running on web: [{0}]", webUrl));

            WithO365Context(webUrl, context =>
            {
                for (var provisionGeneration = 0;
                    provisionGeneration < ProvisionGenerationCount;
                    provisionGeneration++)
                {
                    if (EnableDefinitionProvision)
                        _provisionService.DeployModel(WebModelHost.FromClientContext(context), model);

                    if (EnableDefinitionValidation)
                        _validationService.DeployModel(WebModelHost.FromClientContext(context), model);
                }
            });
        }


        /// <summary>
        /// Deploys and validates target web model.
        /// </summary>
        /// <param name="model"></param>
        public override void DeployWebModel(ModelNode model)
        {
            if (RandomBalancedUrls.Count > 0)
            {
                var url = RandomBalancedUrls[rnd.Next(0, RandomBalancedUrls.Count)];
                WebOnUrl(model, url);
            }
            else
            {
                foreach (var webUrl in WebUrls)
                {
                    Trace.WriteLine(string.Format("[INF]    Running on web: [{0}]", webUrl));



                    for (var provisionGeneration = 0;
                        provisionGeneration < ProvisionGenerationCount;
                        provisionGeneration++)
                    {
                        WithO365Context(webUrl, context =>
                        {
                            if (EnableDefinitionProvision)
                                _provisionService.DeployModel(WebModelHost.FromClientContext(context), model);

                            if (EnableDefinitionValidation)
                                _validationService.DeployModel(WebModelHost.FromClientContext(context), model);

                        });
                    }


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

        /// <summary>
        /// Invokes target action under O365 client context.
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="action"></param>
        public void WithO365Context(string siteUrl, Action<ClientContext> action)
        {
            WithO365Context(siteUrl, UserName, UserPassword, action);
        }

        /// <summary>
        /// Invokes target action under O365 client context.
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <param name="action"></param>
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
