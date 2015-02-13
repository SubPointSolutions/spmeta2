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

using SPMeta2.Exceptions;
using SPMeta2.Models;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ListItemValueScenariousTest : SPMeta2RegresionScenarioTestBase
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

        #region default values

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItemValue")]
        public void CanDeploy_ListItemValue_ById()
        {
            try
            {
                WithListItemValue((listItem, fieldDefs) =>
                {
                    foreach (var fieldDef in fieldDefs)
                    {
                        listItem.AddListItemFieldValue(new ListItemFieldValueDefinition
                        {
                            FieldId = fieldDef.Id,
                            Value = Rnd.String()
                        });
                    }
                });
            }
            catch (SPMeta2NotSupportedException ex)
            {
                Assert.IsTrue(ex.Message.Contains("ListItemFieldValueDefinition.FieldId"));
            }
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItemValue")]
        public void CanDeploy_ListItemValue_ByInternalName()
        {
            WithListItemValue((listItem, fieldDefs) =>
            {
                foreach (var fieldDef in fieldDefs)
                {
                    listItem.AddListItemFieldValue(new ListItemFieldValueDefinition
                    {
                        FieldName = fieldDef.InternalName,
                        Value = Rnd.String()
                    });
                }
            });
        }

        #endregion

        #region utils

        private void WithListItemValue(Action<ModelNode, List<FieldDefinition>> setup)
        {
            var fieldDefs = new List<FieldDefinition>();

            fieldDefs.Add(ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Text;
            }));

            fieldDefs.Add(ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Text;
            }));

            var list = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                foreach (var fieldDef in fieldDefs)
                    site.AddField(fieldDef);
            });

            var webModel = SPMeta2Model
                .NewWebModel(web =>
                {
                    web.AddList(list, rndList =>
                    {
                        foreach (var fieldDef in fieldDefs)
                            rndList.AddListFieldLink(fieldDef);

                        rndList.AddRandomListItem(listItem =>
                        {
                            setup(listItem, fieldDefs);
                        });
                    });

                });

            TestModel(siteModel, webModel);
        }

        #endregion
    }


}
