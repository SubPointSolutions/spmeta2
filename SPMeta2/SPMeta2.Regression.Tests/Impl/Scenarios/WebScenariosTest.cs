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
            var mainWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = string.Empty;
                def.CustomWebTemplate = "{D270F1BE-1943-4064-9509-DA4F14B32228}#M2CustomWebAsTemplate";
            });

            var subWeb = ModelGeneratorService.GetRandomDefinition<WebDefinition>(def =>
            {
                def.WebTemplate = string.Empty;
                def.CustomWebTemplate = "{D270F1BE-1943-4064-9509-DA4F14B32228}#M2CustomWebAsTemplate";
            });

            var model = SPMeta2Model.NewWebModel(rootWeb =>
            {
                rootWeb.AddWeb(mainWeb, web =>
                {
                    web.AddWeb(subWeb);
                });
            });

            TestModel(model);
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

        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Webs")]
        //public void CanDeploy_PublishingSite_Intranet()
        //{
        //    TestRandomDefinition<WebDefinition>(def =>
        //    {
        //        def.WebTemplate = BuiltInWebTemplates.Publishing.PublishingSite_Intranet;
        //    });
        //}

        //[TestMethod]
        //[TestCategory("Regression.Scenarios.Webs")]
        //public void CanDeploy_PublishingSite_CMS()
        //{
        //    TestRandomDefinition<WebDefinition>(def =>
        //    {
        //        def.WebTemplate = BuiltInWebTemplates.Publishing.PublishingSite_CMS;
        //    });
        //}

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
    }
}
