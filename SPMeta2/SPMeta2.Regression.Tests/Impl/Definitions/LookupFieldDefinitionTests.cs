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
    public class LookupFieldDefinitionTests : SPMeta2DefinitionRegresionTestBase
    {
        #region common

        //[ClassInitializeAttribute]
        //public static void Init(TestContext context)
        //{
        //    InternalInit();
        //}

        //[ClassCleanupAttribute]
        //public static void Cleanup()
        //{
        //    InternalCleanup();
        //}

        #endregion

        #region properties

        [TestMethod]
        [TestCategory("Regression.Definitions.LookupFieldDefinition.Properties")]
        [TestCategory("CI.Core")]
        public void LookupFieldDefinition_AllowMultipleValues_ShouldUpdate_FieldType()
        {
            var def = new LookupFieldDefinition();

            def.AllowMultipleValues = true;
            Assert.IsTrue(def.FieldType == BuiltInFieldTypes.LookupMulti);

            def.AllowMultipleValues = false;
            Assert.IsTrue(def.FieldType == BuiltInFieldTypes.Lookup);
        }

        #endregion
    }


}
