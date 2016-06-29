using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Standard.Definitions.Fields;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class TaxonomyFieldDefinitionTests : SPMeta2DefinitionRegresionTestBase
    {
        #region common

        //[ClassInitialize]
        //public static void Init(TestContext context)
        //{
        //    InternalInit();
        //}

        //[ClassCleanup]
        //public static void Cleanup()
        //{
        //    InternalCleanup();
        //}

        #endregion

        #region properties

        [TestMethod]
        [TestCategory("Regression.Definitions.TaxonomyFieldDefinition.Properties")]
        [TestCategory("CI.Core")]
        public void TaxonomyFieldDefinition_AllowMultipleValues_ShouldUpdate_FieldType()
        {
            var def = new TaxonomyFieldDefinition();

            def.IsMulti = false;
            Assert.IsTrue(def.FieldType == BuiltInFieldTypes.TaxonomyFieldType);

            def.IsMulti = true;
            Assert.IsTrue(def.FieldType == BuiltInFieldTypes.TaxonomyFieldTypeMulti);
        }

        #endregion
    }
}
