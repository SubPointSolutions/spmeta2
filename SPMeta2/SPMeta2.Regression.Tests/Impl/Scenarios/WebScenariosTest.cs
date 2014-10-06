using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void CanDeploy_WebHierarchy()
        {
            var model = SPMeta2Model
                             .NewWebModel(web =>
                             {
                                 web
                                     .AddWeb(SampleWebs.Archive)
                                     .AddWeb(SampleWebs.Blog)
                                     .AddWeb(SampleWebs.CIO, cioWeb =>
                                     {
                                         cioWeb
                                             .AddWeb(SampleWebs.Blog);
                                     })
                                     .AddWeb(SampleWebs.Departments, departmentsWeb =>
                                     {
                                         departmentsWeb
                                           .AddWeb(SampleWebs.HR)
                                           .AddWeb(SampleWebs.IT)
                                           .AddWeb(SampleWebs.Delivery)
                                           .AddWeb(SampleWebs.Sales)
                                           .AddWeb(SampleWebs.PR);
                                     })
                                     .AddWeb(SampleWebs.Projects)
                                     .AddWeb(SampleWebs.Wiki)
                                     .AddWeb(SampleWebs.FAQ);

                             });

            TestModel(model);
        }

        #endregion


        #region web tree and subwebs

        // TODO

        #endregion

    }
}
