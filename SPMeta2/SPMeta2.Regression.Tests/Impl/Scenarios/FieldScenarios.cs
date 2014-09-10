using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class FieldScenarios : SPMeta2RegresionEventsTestBase
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

        #region fields

        [TestMethod]
        [TestCategory("Regression.Scenarios.Fields")]
        public void CanDeploy_BooleanField()
        {
            TestRandomDefinition<FieldDefinition>(def =>
            {
                def.FieldType = BuiltInFieldTypes.Boolean;
            });
        }

        #endregion
    }
}
