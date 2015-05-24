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
    public class WelcomePageScenariosTest : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Scenarios.WelcomePage")]
        public void CanDeploy_WelcomePage_ToWeb()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomWelcomePage();
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WelcomePage")]
        public void CanDeploy_WelcomePage_ToList()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomList(list =>
                {
                    list.AddRandomWelcomePage();
                });
            });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.WelcomePage")]
        public void CanDeploy_WelcomePage_ToListFolder()
        {
            var listDefinition = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDefinition, list =>
                {
                    list.AddRandomFolder(folder =>
                    {
                        folder.AddRandomWelcomePage(def =>
                        {
                            (def.Value as WelcomePageDefinition).Url = "Forms/AllItems.aspx";
                        });
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WelcomePage")]
        public void CanDeploy_WelcomePage_ToLibraryFolder()
        {
            var listDefinition = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDefinition, list =>
                {
                    list.AddRandomFolder(folder =>
                    {
                        folder.AddRandomWelcomePage();
                    });
                });
            });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.WelcomePage.Fixes")]
        public void CanDeploy_WelcomePage_WithStartingSlash()
        {
            var listDefinition = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var welcomePage = ModelGeneratorService.GetRandomDefinition<WelcomePageDefinition>(def =>
            {
                def.Url = string.Format("/{0}.aspx", RegressionService.RndService.String());
            });

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDefinition, list =>
                {
                    list.AddRandomFolder(folder =>
                    {
                        folder.AddWelcomePage(welcomePage);
                    });
                });
            });

            TestModel(model);
        }

        #endregion
    }
}
