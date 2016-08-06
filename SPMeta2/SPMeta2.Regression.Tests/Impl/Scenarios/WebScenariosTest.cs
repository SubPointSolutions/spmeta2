using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using System.IO;
using SPMeta2.Containers.Consts;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class WebScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region slash stuff


        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs.Fixes")]
        public void CanDeploy_Web_With_Slash_In_Url()
        {
            TestRandomDefinition<WebDefinition>(def =>
            {
                def.Url = string.Format("/{0}", Rnd.String());
                def.WebTemplate = BuiltInWebTemplates.Collaboration.BlankSite;
            });
        }

        #endregion

        #region templates

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs.Templates")]
        public void CanDeploy_Custom_WebTemplate_As_SaveAsTemplate()
        {
            // TODO
            // should be uploaded manually yet
            var solution = new SandboxSolutionDefinition
            {
                FileName = Rnd.WspFileName(),
                Content = File.ReadAllBytes(DefaultContainers.WebTemplates.M2CustomWebAsTemplate.FilePath),
                Activate = true,
                SolutionId = DefaultContainers.WebTemplates.M2CustomWebAsTemplate.SolutionId
            };

            var mainWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = string.Empty;
                def.CustomWebTemplate = DefaultContainers.WebTemplates.M2CustomWebAsTemplate.WebTemplateName;
            });

            var subWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = string.Empty;
                def.CustomWebTemplate = DefaultContainers.WebTemplates.M2CustomWebAsTemplate.WebTemplateName;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                // required by the team site template
                // might not be eabled due to publishing site collection
                var targetSiteFeatureIds = new[]
                {
                    new Guid("14aafd3a-fcb9-4bb7-9ad7-d8e36b663bbd"),
                    new Guid("b21b090c-c796-4b0f-ac0f-7ef1659c20ae"), 
                    new Guid("{5f3b0127-2f1d-4cfd-8dd2-85ad1fb00bfc}"),
                    new Guid("{2ed1c45e-a73b-4779-ae81-1524e4de467a}"),
                    new Guid("{39d18bbf-6e0f-4321-8f16-4e3b51212393}"),
                    
                    new Guid("{4248e21f-a816-4c88-8cab-79d82201da7b}"),
                    new Guid("{43f41342-1a37-4372-8ca0-b44d881e4434}"),
                    new Guid("{5a979115-6b71-45a5-9881-cdc872051a69}"),
                    new Guid("{3cb475e7-4e87-45eb-a1f3-db96ad7cf313}"),
                    new Guid("{4c42ab64-55af-4c7c-986a-ac216a6e0c0e}"),
                    new Guid("{9fec40ea-a949-407d-be09-6cba26470a0c}"),
                    new Guid("{875d1044-c0cf-4244-8865-d2a0039c2a49}"),
                    new Guid("{5eac763d-fbf5-4d6f-a76b-eded7dd7b0a5}"),
                    new Guid("{6e8f2b8d-d765-4e69-84ea-5702574c11d6}"),
                    new Guid("{744b5fd3-3b09-4da6-9bd1-de18315b045d}"),

                    new Guid("{8581a8a7-cf16-4770-ac54-260265ddb0b2}"),
                    new Guid("{2fcd5f8a-26b7-4a6a-9755-918566dba90a}"),
                    new Guid("{e995e28b-9ba8-4668-9933-cf5c146d7a9f}"),

                    //new Guid("{c9c9515d-e4e2-4001-9050-74f980f93160}"),
                    
                };

                foreach (var featureId in targetSiteFeatureIds)
                {
                    site.AddSiteFeature(new FeatureDefinition
                    {
                        Id = featureId,
                        Enable = true,
                        Scope = FeatureDefinitionScope.Site
                    });
                }

                //site.AddSandboxSolution(solution);
            });

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(mainWeb, web =>
                {
                    web.AddWeb(subWeb);
                });
            });

            TestModel(siteModel, model);
        }

        #endregion

        #region default

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs")]
        public void CanDeploy_BlankWeb()
        {
            TestRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = BuiltInWebTemplates.Collaboration.BlankSite;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs")]
        public void CanDeploy_ProjectWeb()
        {
            TestRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = BuiltInWebTemplates.Collaboration.ProjectSite;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs")]
        public void CanDeploy_BlogWeb()
        {
            TestRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = BuiltInWebTemplates.Collaboration.Blog;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs")]
        public void CanDeploy_CommunityWeb()
        {
            // SocialSite feature needs to be activated
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(new FeatureDefinition
                {
                    Enable = true,
                    Scope = FeatureDefinitionScope.Site,
                    Id = new Guid("4326e7fc-f35a-4b0f-927c-36264b0a4cf0")
                });
            });


            TestModel(siteModel);

            TestRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = BuiltInWebTemplates.Collaboration.CommunitySite;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs")]
        public void CanDeploy_TeamWeb()
        {
            TestRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs")]
        public void CanDeploy_WikiWeb()
        {
            TestRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = BuiltInWebTemplates.Collaboration.WikiSite;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs")]
        public void CanDeploy_PublishingSite_Intranet()
        {
            TestRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = BuiltInWebTemplates.Publishing.PublishingSite_Intranet;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs")]
        public void CanDeploy_PublishingSite_CMS()
        {
            TestRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = BuiltInWebTemplates.Publishing.PublishingSite_CMS;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs")]
        public void CanDeploy_WebHierarchy()
        {
            var model = SPMeta2Model
                             .NewWebModel(web =>
                             {
                                 web
                                     .AddWeb(RegWebs.Archive)
                                     .AddWeb(RegWebs.Blog)
                                     .AddWeb(RegWebs.CIO, cioWeb =>
                                     {
                                         cioWeb
                                             .AddWeb(RegWebs.CIOBlog);
                                     })
                                     .AddWeb(RegWebs.Departments, departmentsWeb =>
                                     {
                                         departmentsWeb
                                           .AddWeb(RegWebs.HR)
                                           .AddWeb(RegWebs.IT)
                                           .AddWeb(RegWebs.Delivery)
                                           .AddWeb(RegWebs.Sales)
                                           .AddWeb(RegWebs.PR);
                                     })
                                     .AddWeb(RegWebs.Projects)
                                     .AddWeb(RegWebs.Wiki)
                                     .AddWeb(RegWebs.FAQ);

                             });

            TestModel(model);
        }

        #endregion

        #region web tree and subwebs

        // TODO

        #endregion

        #region webs localization

        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs.Localization")]
        public void CanDeploy_Localized_Web()
        {
            var definition = GetLocalizedWebDefinition();

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(definition);
            });

            TestModel(model);
        }

        #endregion

        #region indexed properties


        [TestMethod]
        [TestCategory("Regression.Scenarios.Webs.IndexedProps")]
        public void CanDeploy_Web_WithIndexed_Props()
        {
            var webDef = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {
                def.IndexedPropertyKeys.Add(new IndexedPropertyValue
                {
                    Name = string.Format("name_{0}", Rnd.String()),
                    Value = string.Format("value_{0}", Rnd.String()),
                });

                def.IndexedPropertyKeys.Add(new IndexedPropertyValue
                {
                    Name = string.Format("name_{0}", Rnd.String()),
                    Value = string.Format("value_{0}", Rnd.String()),
                });
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWeb(webDef);
            });

            TestModel(model);
        }

        #endregion

        #region customizations

        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Webs.Customizations")]
        //public void CanDeploy_ProjectWeb_With_Customizations()
        //{
        //    https://github.com/SubPointSolutions/spmeta2/issues/860 
        //    var webDef = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
        //    {
        //        def.WebTemplate = BuiltInWebTemplates.Collaboration.ProjectSite;
        //    });

        //    var model = SPMeta2Model.NewWebModel(rootWeb =>
        //    {
        //        rootWeb.AddRandomList();
        //        rootWeb.AddRandomDocumentLibrary();

        //        rootWeb.AddRandomField();

        //        rootWeb.AddRandomWeb();

        //        rootWeb.AddWeb(webDef, web =>
        //        {
        //            web.AddRandomList();
        //            web.AddRandomDocumentLibrary();

        //            web.AddRandomField();

        //            web.AddRandomWeb();
        //        });
        //    });

        //    TestModel(model);
        //}

        #endregion


        #region utils

        protected WebDefinition GetLocalizedWebDefinition()
        {
            var definition = ModelGeneratorService.GetRandomDefinition<WebDefinition>();
            return GetLocalizedWebDefinition(definition);
        }

        protected WebDefinition GetLocalizedWebDefinition(WebDefinition definition)
        {
            var localeIds = Rnd.LocaleIds();

            foreach (var localeId in localeIds)
            {
                definition.TitleResource.Add(new ValueForUICulture
                {
                    CultureId = localeId,
                    Value = string.Format("LocalizedTitle_{0}", localeId)
                });

                definition.DescriptionResource.Add(new ValueForUICulture
                {
                    CultureId = localeId,
                    Value = string.Format("LocalizedDescription_{0}", localeId)
                });
            }

            return definition;
        }

        #endregion
    }
}
