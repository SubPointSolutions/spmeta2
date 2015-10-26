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

        #region field values

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_ListItem_With_FieldValues()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_ListItem_With_RequiredFieldValues()
        {
            Assert.IsTrue(false);
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_ListItem_With_ContentType_ByName()
        {
            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var itemDef = ModelGeneratorService.GetRandomDefinition<ListItemDefinition>(def =>
            {
                def.ContentTypeName = contentTypeDef.Name;
            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddContentType(contentTypeDef);
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddList(listDef, list =>
                {
                    list.AddContentTypeLink(contentTypeDef);
                    list.AddListItem(itemDef);
                });
            });

            TestModel(siteModel, webModel);
        }

        #endregion

        #region default list

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems")]
        public void CanDeploy_ListItem_ToList()
        {
            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.EnableMinorVersions = false;
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
                def.EnableMinorVersions = false;
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
