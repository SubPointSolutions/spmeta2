using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class ScriptEditorWebPartDefinitionTests : SPMeta2RegresionScenarioTestBase
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

        #region tests

        [TestMethod]
        [TestCategory("Regression.Definitions.ScriptEditorWebPartDefinition")]
        [ExpectedException(typeof(SPMeta2InvalidDefinitionPropertyException))]
        public void ScriptEditorWebPartDefinition_Id_LessThan_32_ShouldFail()
        {
            var def = new ScriptEditorWebPartDefinition
            {
                Id = Rnd.String(31)
            };
        }

        [TestMethod]
        [TestCategory("Regression.Definitions.ScriptEditorWebPartDefinition")]
        public void ScriptEditorWebPartDefinition_Id_LessThan_32_ShouldPass()
        {
            var def = new ScriptEditorWebPartDefinition
            {
                Id = Rnd.String(33)
            };
        }

        #endregion
    }
}
