using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
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
    public class ListItemScenariousTest : SPMeta2RegresionScenarioTestBase
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

        #region default list

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems")]
        public void CanDeploy_ListItem_ToList()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        rndList.AddRandomListItem();
                    });

                });

            TestModel(model);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems")]
        public void CanDeploy_ListItem_ToListFolder()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        rndList.AddRandomFolder(rndFolder =>
                        {
                            rndFolder.AddRandomListItem();
                        });
                    });

                });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems")]
        public void CanDeploy_ListItem_ToListSubFolder()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var model = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        rndList.AddRandomFolder(rndFolder =>
                        {
                            rndFolder.AddRandomFolder(rndSubFolder =>
                            {
                                rndSubFolder.AddRandomListItem();
                            });
                           
                        });
                    });

                });

            TestModel(model);
        }

        #endregion
    }
}
