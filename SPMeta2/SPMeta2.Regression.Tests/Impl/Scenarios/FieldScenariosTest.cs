using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class FieldScenariosTest : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public FieldScenariosTest()
        {
            ProvisionGenerationCount = 2;
        }

        #endregion

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
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_BooleanField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Boolean;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_CalculatedField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Calculated;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_ChoiceField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Choice;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_ComputedField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Computed;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_CurrencyField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Currency;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_DateTimeField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.DateTime;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_GeolocationField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Geolocation;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_GuidField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Guid;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_LookupField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Lookup;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_MultiChoiceField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.MultiChoice;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_NoteField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Note;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_NumberField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Number;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_OutcomeChoiceField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.OutcomeChoice;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_TaxonomyFieldTypeField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.TaxonomyFieldType;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_TaxonomyFieldTypeMultiField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.TaxonomyFieldTypeMulti;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_TextField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Text;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_URLField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.URL;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_UserField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.User;
            });
        }

        #endregion

        #region field to site

        // TODO

        #endregion

        #region field to list

        // TODO

        #endregion
    }
}
