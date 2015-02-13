using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Validators.Relationships;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class WikiPageScenariosTest : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Scenarios.WikiPages")]
        public void CanDeploy_WikiPage()
        {
            WithSitePagesList(sitePages =>
            {
                sitePages.AddRandomWikiPage();
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WikiPages")]
        public void CanDeploy_WikiPageToFolder()
        {
            WithSitePagesList(sitePages =>
            {
                sitePages.AddRandomFolder(folder =>
                {
                    folder.AddRandomWikiPage();
                });
            });
        }

        #endregion

        #region utils

        private void WithSitePagesList(Action<ModelNode> pagesList)
        {
            var webModel = SPMeta2Model
               .NewWebModel(web =>
               {
                   web
                       .AddFeature(BuiltInWebFeatures.WikiPageHomePage.Inherit(def =>
                       {
                           def.Enable = true;
                       }))
                       .AddHostList(BuiltInListDefinitions.SitePages, list =>
                       {
                           pagesList(list);
                       });
               });

            TestModel(webModel);
        }

        #endregion

    }
}
