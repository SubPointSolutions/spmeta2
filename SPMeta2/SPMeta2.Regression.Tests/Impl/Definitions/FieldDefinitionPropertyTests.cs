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
using SPMeta2.Containers.DefinitionGenerators.Fields;
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

                // dep lookiup
                if (indexedSiteField is DependentLookupFieldDefinition)
                {
                    var primaryLookupField = new LookupFieldDefinitionGenerator().GenerateRandomDefinition() as LookupFieldDefinition;

                    (indexedSiteField as DependentLookupFieldDefinition).PrimaryLookupFieldId = primaryLookupField.Id;
                    indexedSiteModel.AddField(primaryLookupField);
                }

                TestModel(indexedSiteModel);

                Trace.WriteLine(string.Format("Checking Indexed prop for non-Indexed def:[{0}]", fieldDefintion.GetType().Name));

                var nonIdexedSiteModel = SPMeta2Model.NewSiteModel(m => { });
                var nonIndexedSiteField = ModelGeneratorService.GetRandomDefinition(fieldDefintion) as FieldDefinition;

                nonIdexedSiteModel.AddField(nonIndexedSiteField);
                nonIndexedSiteField.Indexed = false;

                // dep lookiup
                if (indexedSiteField is DependentLookupFieldDefinition)
                {
                    var primaryLookupField = new LookupFieldDefinitionGenerator().GenerateRandomDefinition() as LookupFieldDefinition;

                    (nonIndexedSiteField as DependentLookupFieldDefinition).PrimaryLookupFieldId = primaryLookupField.Id;
                    nonIdexedSiteModel.AddField(primaryLookupField);
                }

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

                // dep lookiup
                if (siteField is DependentLookupFieldDefinition)
                {
                    var primaryLookupField = new LookupFieldDefinitionGenerator().GenerateRandomDefinition() as LookupFieldDefinition;

                    (siteField as DependentLookupFieldDefinition).PrimaryLookupFieldId = primaryLookupField.Id;
                    siteModel.AddField(primaryLookupField);
                }

                siteField.ValidationMessage = string.Format("validatin_msg_{0}", RegressionService.RndService.String());
                siteField.ValidationFormula = string.Format("=[ID] * {0}", RegressionService.RndService.Int(100));

                TestModel(siteModel);
            }
        }

        #endregion
    }
}
