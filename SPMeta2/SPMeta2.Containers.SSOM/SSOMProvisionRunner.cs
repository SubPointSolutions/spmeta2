using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using Microsoft.Office.SecureStoreService.Server;
using Microsoft.Office.Server.Audience;
using Microsoft.Office.Server.Search.Portability;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Publishing.Navigation;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.WorkflowServices;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Consts;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Utils;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using SPMeta2.Regression.CSOM.Validation.Webparts;
using SPMeta2.Regression.SSOM;
using SPMeta2.Regression.SSOM.Standard.Validation;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Services;
using SPMeta2.SSOM.Standard.ModelHandlers.Webparts;
using SPMeta2.Utils;
using SPMeta2.Services.Impl;
using SPMeta2.Services.Impl.Validation;
using SPMeta2.SSOM.Standard.Services;
using SPMeta2.ModelHosts;
using SPMeta2.Exceptions;

namespace SPMeta2.Containers.SSOM
{
    public class SSOMProvisionRunner : ProvisionRunnerBase
    {
        #region constructors

        public SSOMProvisionRunner()
        {
            if (!Environment.Is64BitProcess)
            {
                throw new SPMeta2Exception("Environment.Is64BitProcess is false. SSOMProvisionRunner runs SSOM based stuff requiring x64 running process. If you run unit tests from Visual Studio, ensure 'Test -> Test Setting -> Default Processor Architecture -> x64'");
            }

            Name = "SSOM";

            WebApplicationUrls = new List<string>();
            SiteUrls = new List<string>();
            WebUrls = new List<string>();

            LoadEnvironmentConfig();
            InitServices();

            EnableParallelMode = false;
        }

        public bool EnableParallelMode { get; set; }

        private void InitServices()
        {
            _provisionService = new StandardSSOMProvisionService();
            _validationService = new SSOMValidationService();

            // TODO, setup a high level validation registration
            _provisionService.PreDeploymentServices.Add(new DefaultRequiredPropertiesValidationService());

            var ssomStandartAsm = typeof(ContactFieldControlModelHandler).Assembly;

            foreach (var handlerType in ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(ssomStandartAsm))
                _provisionService.RegisterModelHandler(Activator.CreateInstance(handlerType) as ModelHandlerBase);

            var ssomStandartValidationAsm = typeof(ImageRenditionDefinitionValidator).Assembly;

            foreach (var handlerType in ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(ssomStandartValidationAsm))
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

        private void LoadEnvironmentConfig()
        {
            WebApplicationUrls.Clear();
            WebApplicationUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.SSOM_WebApplicationUrls));

            SiteUrls.Clear();
            SiteUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.SSOM_SiteUrls));

            WebUrls.Clear();
            WebUrls.AddRange(RunnerEnvironmentUtils.GetEnvironmentVariables(EnvironmentConsts.SSOM_WebUrls));
        }

        #endregion

        #region properties

        public List<string> WebApplicationUrls { get; set; }
        public List<string> SiteUrls { get; set; }
        public List<string> WebUrls { get; set; }

        private SSOMProvisionService _provisionService;
        private SSOMValidationService _validationService;

        public override string ResolveFullTypeName(string typeName, string assemblyName)
        {
            var sp = typeof(SPField);
            var wp = typeof(WebPart);
            var publishing = typeof(WebNavigationSettings);

            var tax = typeof(TaxonomyField);
            var wf = typeof(WorkflowDefinition);

            var secureStore = typeof(ISecureStore);

            var search = typeof(SearchConfigurationPortability);

            var up = typeof(Audience);

            return base.ResolveFullTypeName(typeName, assemblyName);
        }

        #endregion

        #region methods

        public override void DeployFarmModel(ModelNode model)
        {
            for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
            {
                WithSSOMFarmContext(farm =>
                {
                    if (EnableDefinitionProvision)
                        _provisionService.DeployModel(FarmModelHost.FromFarm(farm), model);

                    if (EnableDefinitionValidation)
                        _validationService.DeployModel(FarmModelHost.FromFarm(farm), model);
                });
            }
        }

        public override void DeployWebApplicationModel(ModelNode model)
        {
            if (!WebApplicationUrls.Any())
                throw new SPMeta2Exception("WebApplicationUrls is empty");

            foreach (var webAppUrl in WebApplicationUrls)
            {
                Trace.WriteLine(string.Format("[INF]    Running on web app: [{0}]", webAppUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithSSOMWebApplicationContext(webAppUrl, webApp =>
                    {
                        if (EnableDefinitionProvision)
                            _provisionService.DeployModel(WebApplicationModelHost.FromWebApplication(webApp), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(WebApplicationModelHost.FromWebApplication(webApp), model);

                    });
                }
            }
        }

        protected string GetScopeHash()
        {
            var frames = new StackTrace().GetFrames();

            foreach (var frame in frames)
            {
                var method = frame.GetMethod();

                var methodClass = method.DeclaringType.AssemblyQualifiedName;
                var methodName = method.Name;

                var siteIsolation = frame.GetMethod().GetCustomAttributes(true)
                    .ToList()
                    .FirstOrDefault(a => a is SiteCollectionIsolationAttribute);

                if (siteIsolation != null)
                {
                    return string.Format("{0}{1}", methodName, methodClass);
                }
            }

            return string.Empty;
        }


        protected string GetTargetSiteCollectionUrl()
        {
            if (!EnableParallelMode)
            {
                return SiteUrls.First();
            }

            var scopeHash = GetScopeHash();

            if (string.IsNullOrEmpty(scopeHash))
            {
                return SiteUrls.First();
            }

            var mappings = RestoreMappings();
            var currentMapping = mappings.FirstOrDefault(m => m.Contains(scopeHash));

            if (currentMapping == null)
            {
                var lastMappingIndex = GetLastIndex();

                if (lastMappingIndex == 9)
                {
                    lastMappingIndex = 0;
                }
                else
                {
                    lastMappingIndex++;
                }

                SaveLastIndex(lastMappingIndex);


                var url = string.Format("http://DEV42:31416/sites/r-{0}", lastMappingIndex);
                var fullMapping = string.Format("{0}|{1}", scopeHash, url);

                mappings.Add(fullMapping);

                SaveMappings(mappings);
                mappings = RestoreMappings();

                currentMapping = mappings.FirstOrDefault(m => m.Contains(scopeHash));
            }


            return currentMapping.Split(new string[] { "|" }, StringSplitOptions.None)[1];

        }

        private void SaveLastIndex(int lastMappingIndex)
        {

            File.WriteAllText("regresion-mapping-index.txt", lastMappingIndex.ToString());
        }

        private long fSize = 1024 * 1024 * 10;


        private int GetLastIndex()
        {
            var result = 0;


            if (File.Exists("regresion-mapping-index.txt"))
            {
                var value = File.ReadAllText("regresion-mapping-index.txt").Trim();

                if (!string.IsNullOrEmpty(value))
                {
                    result = int.Parse(value);
                }
            }



            return result;
        }

        private void SaveMappings(List<string> mappings)
        {
            var value = XmlSerializerUtils.SerializeToString(mappings);

            File.WriteAllText("regresion-mapping.txt", value);

        }

        private List<string> RestoreMappings()
        {
            var fileName = "regresion-mapping.txt";

            var result = new List<string>();
            if (File.Exists(fileName))
            {
                var value = File.ReadAllText(fileName).Trim();

                if (!string.IsNullOrEmpty(value))
                {
                    result = XmlSerializerUtils.DeserializeFromString<List<string>>(value);
                }
            }

            return result;
        }


        public override void DeploySiteModel(ModelNode model)
        {
            var scope = GetScopeHash();

            if (!SiteUrls.Any())
                throw new SPMeta2Exception("SiteUrls is empty");

            foreach (var siteUrl in SiteUrls)
            {
                //var siteUrl = GetTargetSiteCollectionUrl();

                Trace.WriteLine(string.Format("[INF]    Running on site: [{0}]", siteUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithSSOMSiteAndWebContext(siteUrl, (site, web) =>
                    {
                        if (EnableDefinitionProvision)
                            _provisionService.DeployModel(SiteModelHost.FromSite(site), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(SiteModelHost.FromSite(site), model);
                    });
                }
            }
        }

        public override void DeployWebModel(ModelNode model)
        {
            if (!WebUrls.Any())
                throw new SPMeta2Exception("WebUrls is empty");

            foreach (var webUrl in WebUrls)
            {
                //var webUrl = GetTargetSiteCollectionUrl();


                Trace.WriteLine(string.Format("[INF]    Running on web: [{0}]", webUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithSSOMSiteAndWebContext(webUrl, (site, web) =>
                    {
                        if (EnableDefinitionProvision)
                            _provisionService.DeployModel(WebModelHost.FromWeb(web), model);

                        if (EnableDefinitionValidation)
                            _validationService.DeployModel(WebModelHost.FromWeb(web), model);
                    });
                }
            }
        }



        public override void DeployListModel(ModelNode model)
        {
            foreach (var webUrl in WebUrls)
            {
                Trace.WriteLine(string.Format("[INF]    Running on web: [{0}]", webUrl));

                for (var provisionGeneration = 0; provisionGeneration < ProvisionGenerationCount; provisionGeneration++)
                {
                    WithSSOMSiteAndWebContext(webUrl, (site, web) =>
                    {
                        var list = web.Lists.TryGetList("Site Pages");

                        if (list == null)
                        {
                            list = web.Lists.TryGetList("Pages");
                        }

                        if (list == null)
                        {
                            throw new SPMeta2Exception("Cannot find host list");
                        }

                        if (EnableDefinitionProvision)
                            _provisionService.DeployListModel(list, model);

                        if (EnableDefinitionValidation)
                        {
                            var listHost = ModelHostBase.Inherit<ListModelHost>(WebModelHost.FromWeb(list.ParentWeb), h =>
                            {
                                h.HostList = list;
                            });

                            _validationService.DeployModel(listHost, model);
                        }
                    });
                }
            }
        }

        #endregion

        #region utils

        public void WithSSOMWebApplicationContext(string webAppUrl, Action<SPWebApplication> action)
        {
            var webApp = SPWebApplication.Lookup(new Uri(webAppUrl));

            action(webApp);
        }

        public void WithSSOMFarmContext(Action<SPFarm> action)
        {
            var farm = SPFarm.Local;

            action(farm);
        }

        public void WithSSOMSiteAndWebContext(string siteUrl, Action<SPSite, SPWeb> action)
        {
            using (var site = new SPSite(siteUrl))
            {
                using (var web = site.OpenWeb())
                {
                    action(site, web);
                }
            }
        }

        #endregion
    }
}
