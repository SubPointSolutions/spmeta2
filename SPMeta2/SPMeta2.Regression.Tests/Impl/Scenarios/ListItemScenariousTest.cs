using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SPMeta2.Definitions.Fields;
using SPMeta2.Regression.Tests.Prototypes;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;
using SPMeta2.Regression.Utils;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class ListItemScenariousSelfTest
    {
        #region default values

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        [TestCategory("CI.Core")]
        public void SelfDiagnostic_AllFields_Should_Have_Value_Tests()
        {
            var isValid = true;

            var methods = typeof(ListItemScenariousTest).GetMethods();
            var targetTypes = new List<Type>();

            var assemblies = new[]
            {
                typeof(FieldDefinition).Assembly,
                typeof(TaxonomyFieldDefinition).Assembly
            };

            targetTypes.AddRange(ReflectionUtils.GetTypesFromAssemblies<FieldDefinition>(assemblies));

            RegressionUtils.WriteLine(string.Format("Found [{0}] fied types.", targetTypes.Count));

            foreach (var fieldType in targetTypes.OrderBy(m => m.Name))
            {
                var fullDefName = fieldType.Name;
                var shortDefName = fieldType.Name.Replace("Definition", string.Empty);

                var testName1 = string.Format("CanDeploy_{0}_Value", shortDefName);
                var testName2 = string.Format("CanDeploy_{0}_Values", shortDefName);

                var testExists1 = methods.Any(m => m.Name == testName1);
                var testExists2 = methods.Any(m => m.Name == testName2);


                if (!testExists1)
                    isValid = false;

                if (!testExists2)
                    isValid = false;

                RegressionUtils.WriteLine(string.Format("[{0}] def: {1} one value test method: {2}",
                        testExists1, fullDefName, testName1));

                RegressionUtils.WriteLine(string.Format("[{0}] def: {1} multiple values test method: {2}",
                        testExists2, fullDefName, testName2));

                RegressionUtils.WriteLine(string.Empty);
            }

            Assert.IsTrue(isValid);
        }
        #endregion
    }

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
        public void CanDeploy_ListItem_With_RequiredFieldValues()
        {
            var requiredText = RItemValues.GetRequiredTextField(ModelGeneratorService);

            var text1 = RItemValues.GetRandomTextField(ModelGeneratorService);
            var text2 = RItemValues.GetRandomTextField(ModelGeneratorService);

            var contentTypeDef = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var itemDef = ModelGeneratorService.GetRandomDefinition<ListItemDefinition>(def =>
            {
                def.ContentTypeName = contentTypeDef.Name;

                def.DefaultValues.Add(new FieldValue()
                {
                    FieldName = requiredText.InternalName,
                    Value = Rnd.String()
                });

                def.Values.Add(new FieldValue()
                {
                    FieldName = text1.InternalName,
                    Value = Rnd.String()
                });

                def.Values.Add(new FieldValue()
                {
                    FieldName = text2.InternalName,
                    Value = Rnd.String()
                });

            });

            var listDef = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(requiredText);
                site.AddField(text1);
                site.AddField(text2);

                site.AddContentType(contentTypeDef, contentType =>
                {
                    contentType.AddContentTypeFieldLink(requiredText);
                    contentType.AddContentTypeFieldLink(text1);
                    contentType.AddContentTypeFieldLink(text2);
                });

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

        #region field values


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_BooleanField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_BooleanField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_BusinessDataField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_BusinessDataField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_CalculatedField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_CalculatedField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_ChoiceField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_ChoiceField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_ComputedField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_ComputedField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_CurrencyField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_CurrencyField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_DateTimeField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_DateTimeField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_DependentLookupField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_DependentLookupField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_Field_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_Field_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_GeolocationField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_GeolocationField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_GuidField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_GuidField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_HTMLField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_HTMLField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_ImageField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_ImageField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_LinkField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_LinkField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_LookupField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_LookupField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_MediaField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_MediaField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_MultiChoiceField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_MultiChoiceField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_NoteField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_NoteField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_NumberField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_NumberField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_OutcomeChoiceField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_OutcomeChoiceField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_SummaryLinkField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_SummaryLinkField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_TaxonomyField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_TaxonomyField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_TextField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_TextField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_URLField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_URLField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_UserField_Value()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ListItems.Values")]
        public void CanDeploy_UserField_Values()
        {
            // Cover ListItemFieldValueDefinition/ListItemFieldValuesDefinition with regression test #533
            // https://github.com/SubPointSolutions/spmeta2/issues/533

            throw new NotImplementedException();
        }

        #endregion
    }
}
