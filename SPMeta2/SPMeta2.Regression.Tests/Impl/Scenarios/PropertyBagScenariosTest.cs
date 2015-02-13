using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{

    [TestClass]
    public class PropertyBagScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region property bad cases

        [TestMethod]
        [TestCategory("Regression.Scenarios.PropertyBags")]
        public void CanDeploy_PropertyBag_OnFarm()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var model = SPMeta2Model
                        .NewFarmModel(farm =>
                        {
                            farm.AddRandomPropertyBag();
                        });

                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.PropertyBags")]
        public void CanDeploy_PropertyBag_OnWebApplication()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var model = SPMeta2Model
                        .NewWebApplicationModel(webApp =>
                        {
                            webApp.AddRandomPropertyBag();
                        });

                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.PropertyBags")]
        public void CanDeploy_PropertyBag_OnWeb()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddRandomPropertyBag();
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PropertyBags")]
        public void CanDeploy_PropertyBag_OnList()
        {
            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddRandomList(rndList =>
                        {
                            rndList.AddRandomPropertyBag();
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PropertyBags")]
        public void CanDeploy_PropertyBag_OnListFolder()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, rndList =>
                        {
                            rndList.AddRandomFolder(rndFolder =>
                            {
                                rndFolder.AddRandomPropertyBag();
                            });
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PropertyBags")]
        public void CanDeploy_PropertyBag_OnLibraryFolder()
        {
            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.EnableAttachments = false;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddRandomWeb(rndWeb =>
                    {
                        rndWeb.AddList(listDef, rndList =>
                        {
                            rndList.AddRandomFolder(rndFolder =>
                            {
                                rndFolder.AddRandomPropertyBag();
                            });
                        });
                    });
                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.PropertyBags")]
        public void CanDeploy_PropertyBag_OnListItem()
        {
            WithExcpectedExceptions(new[] { typeof(SPMeta2NotImplementedException) }, () =>
            {
                var model = SPMeta2Model
                    .NewWebModel(web =>
                    {
                        web.AddRandomWeb(rndWeb =>
                        {
                            rndWeb.AddRandomList(rndList =>
                            {
                                rndList.AddRandomListItem(rndListItem =>
                                {
                                    rndListItem.AddRandomPropertyBag();
                                });
                            });
                        });
                    });


                TestModel(model);
            });
        }


        #endregion

    }

}
