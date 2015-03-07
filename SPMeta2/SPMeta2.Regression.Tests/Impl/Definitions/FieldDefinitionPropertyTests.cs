using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Regression.Tests.Base;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class FieldDefinitionPropertyTests : SPMeta2RegresionTestBase
    {
        #region common

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

        #region properties

        [TestMethod]
        [TestCategory("Regression.Definitions.FieldDefinitions.Properties")]
        public void FieldDefinitions_ShouldHave_Correct_Indexed_Property()
        {
            var fieldDefinitionTypes = new List<Type>();

            fieldDefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<FieldDefinition>(typeof(FieldDefinition).Assembly));
            fieldDefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<FieldDefinition>(typeof(TaxonomyFieldDefinition).Assembly));

            foreach (var fieldDefintion in fieldDefinitionTypes)
            {
                Trace.WriteLine(string.Format("Checking Indexed prop for Indexed def:[{0}]", fieldDefintion.GetType().Name));

                var indexedSiteModel = SPMeta2Model.NewSiteModel(m => { });
                var indexedSiteField = ModelGeneratorService.GetRandomDefinition(fieldDefintion) as FieldDefinition;

                indexedSiteModel.AddField(indexedSiteField);
                indexedSiteField.Indexed = true;
                TestModel(indexedSiteModel);

                Trace.WriteLine(string.Format("Checking Indexed prop for non-Indexed def:[{0}]", fieldDefintion.GetType().Name));

                var nonIdexedSiteModel = SPMeta2Model.NewSiteModel(m => { });
                var nonIndexedSiteField = ModelGeneratorService.GetRandomDefinition(fieldDefintion) as FieldDefinition;

                nonIdexedSiteModel.AddField(nonIndexedSiteField);
                nonIndexedSiteField.Indexed = false;
                TestModel(nonIdexedSiteModel);
            }
        }

        [TestMethod]
        [TestCategory("Regression.Definitions.FieldDefinitions.Properties")]
        public void FieldDefinitions_ShouldHave_Correct_ValidationMessageAndFormula_Property()
        {
            var fieldDefinitionTypes = new List<Type>();

            fieldDefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<FieldDefinition>(typeof(FieldDefinition).Assembly));
            fieldDefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<FieldDefinition>(typeof(TaxonomyFieldDefinition).Assembly));

            foreach (var fieldDefintion in fieldDefinitionTypes)
            {
                Trace.WriteLine(string.Format("Checking Indexed propr for Indexed def:[{0}]", fieldDefintion.GetType().Name));

                var siteModel = SPMeta2Model.NewSiteModel(m => { });
                var siteField = ModelGeneratorService.GetRandomDefinition(fieldDefintion) as FieldDefinition;

                siteModel.AddField(siteField);

                siteField.ValidationMessage = string.Format("validatin_msg_{0}", RegressionService.RndService.String());
                siteField.ValidationFormula = string.Format("=[ID] * {0}", RegressionService.RndService.Int(100));

                TestModel(siteModel);
            }
        }

        [TestMethod]
        [TestCategory("TMP")]
        public void TMP()
        {

            var EntityIdPrefix = ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                //def.Title = "EntityIdPrefix";
                //def.Id = new Guid("ceb37b03-ddad-4fe8-8642-a79aa0b1fa24");
                //def.InternalName = "iname_744ea8a43aa84af1";

                def.Hidden = false;
                def.ShowInNewForm = true;
                def.ShowInListSettings = true;

                def.Group = "_my";

                def.FieldType = BuiltInFieldTypes.Text;
            });

            var EntityIdCount = ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                //def.Title = "EntityIdCount";
                //def.Id = new Guid("cf56f958-f487-4f3c-b3dd-3e2cd41749e1");
                //def.InternalName = "iname_b85e0fa0dc014e91";

                def.Hidden = false;
                def.ShowInNewForm = true;
                def.ShowInListSettings = true;

                def.Group = "_my";

                def.FieldType = BuiltInFieldTypes.Number;
            });

            var EntityIdLength = ModelGeneratorService.GetRandomDefinition<FieldDefinition>(def =>
            {
                //def.Title = "EntityIdLength";
                //def.Id = new Guid("ba78babb-46e1-40e6-841c-ecb680ca2487");
                // def.InternalName = "iname_591ccfd599a842c9";

                def.Hidden = false;
                def.ShowInNewForm = true;
                def.ShowInListSettings = true;

                def.Group = "_my";

                def.FieldType = BuiltInFieldTypes.Number;
            });

            //var NFEntityNextId = new CalculatedFieldDefinition
            //{
            //    Title = "NextId",
            //    Id = new Guid("1ACE19DB-87C4-4E6C-BEBB-1FD8DFA9D5C0"),
            //    InternalName = "NFEntityNextId",
            //    FieldType = BuiltInFieldTypes.Calculated,
            //    Formula = string.Format(@"=CONCATENATE([{0}],""_"",REPT(""0"",[{1}]-LEN([{2}]+1)),[{3}]+1)", EntityIdPrefix.InternalName, EntityIdLength.InternalName, EntityIdCount.InternalName, EntityIdCount.InternalName),
            //    FieldReferences = new Collection<string> { EntityIdPrefix.Title, EntityIdCount.Title, EntityIdLength.Title },
            //    OutputType = BuiltInFieldTypes.Text,
            //    CurrencyLocaleId = 1033,
            //    DateFormat = BuiltInDateTimeFieldFormatType.DateTime,
            //    // ShowAsPercentage = false,
            //    Group = "tmp"
            //};

            var NFEntityNextId = ModelGeneratorService.GetRandomDefinition<CalculatedFieldDefinition>(def =>
            {

                //def.FieldType = BuiltInFieldTypes.Calculated;
                def.Group = "_my";
                def.Formula = string.Format(@"=CONCATENATE([{0}],""_"",REPT(""0"",[{1}]-LEN([{2}]+1)),[{3}]+1)",
                    EntityIdPrefix.InternalName, EntityIdLength.InternalName, EntityIdCount.InternalName,
                    EntityIdCount.InternalName);
                def.FieldReferences = new Collection<string>
                {
                    EntityIdPrefix.InternalName,
                    EntityIdCount.InternalName,
                    EntityIdLength.InternalName
                };

                def.Title = "test calc content 1" + Environment.TickCount.ToString();

                def.Hidden = false;
                def.ShowInNewForm = true;
                def.ShowInListSettings = true;

                def.OutputType = BuiltInFieldTypes.Number;
            });

            var testContentType = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.Name = "test calc content 1" + Environment.TickCount.ToString();
                //def.Id = new Guid("632c66d1-7131-4920-8c7d-1c17294bb47a");
                def.Id = Guid.NewGuid();
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });

            var testList = ModelGeneratorService.GetRandomDefinition<ListDefinition>(def =>
            {
                def.TemplateType = BuiltInListTemplateTypeId.GenericList;
                def.Title = "test calc";
                def.CustomUrl = "lists/test-calc";
                def.ContentTypesEnabled = true;
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site
                    .AddField(EntityIdPrefix)
                    .AddField(EntityIdCount)
                    .AddField(EntityIdLength)

                    .AddField(NFEntityNextId)

                    .AddContentType(testContentType, contentType =>
                    {
                        contentType
                            .AddContentTypeFieldLink(EntityIdPrefix)
                            .AddContentTypeFieldLink(EntityIdCount)
                            .AddContentTypeFieldLink(EntityIdLength)
                            .AddContentTypeFieldLink(NFEntityNextId);
                    });
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web
                    .AddList(testList, list =>
                    {
                        list.AddContentTypeLink(testContentType);
                    });
            });

            RegressionService.EnableEventValidation = false;
            RegressionService.EnableDefinitionValidation = false;
            RegressionService.EnablePropertyValidation = false;

            TestModel(siteModel, webModel);
        }

        #endregion
    }
}
