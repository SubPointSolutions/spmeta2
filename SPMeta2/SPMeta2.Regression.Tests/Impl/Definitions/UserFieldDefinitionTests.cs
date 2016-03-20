using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class UserFieldDefinitionTests : SPMeta2DefinitionRegresionTestBase
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
        [TestCategory("Regression.Definitions.UserFieldDefinition.Properties")]
        [TestCategory("CI.Core")]
        public void UserFieldDefinition_AllowMultipleValues_ShouldUpdate_FieldType()
        {
            var def = new UserFieldDefinition();

            def.AllowMultipleValues = false;
            Assert.IsTrue(def.FieldType == BuiltInFieldTypes.User);

            def.AllowMultipleValues = true;
            Assert.IsTrue(def.FieldType == BuiltInFieldTypes.UserMulti);
        }

        #endregion
    }
}
