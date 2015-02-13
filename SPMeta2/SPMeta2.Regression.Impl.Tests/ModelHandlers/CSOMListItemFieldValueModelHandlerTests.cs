using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Impl.Tests.ModelHandlers.Base;

namespace SPMeta2.Regression.Impl.Tests.ModelHandlers
{
    [TestClass]
    public class CSOMListItemFieldValueModelHandlerTests : ModelHandlerTestBase
    {
        #region init

        [ClassInitialize]
        public static void Init(TestContext context)
        {

        }

        [ClassCleanup]
        public static void Cleanup()
        {

        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.CSOM.ListItemFieldValueModelHandler")]
        public void ShouldNotDeploy_CSOM_ListItemFieldValue_WithFieldId()
        {
            var hasException = false;

            var def = new ListItemFieldValueDefinition
            {
                FieldId = BuiltInFieldId.Title,
                Value = Rnd.String()
            };

            try
            {
                var handler = new SPMeta2.CSOM.ModelHandlers.ListItemFieldValueModelHandler();

                handler.DeployModel(null, def);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(SPMeta2NotSupportedException));
                Assert.IsTrue(ex.Message.Contains("ListItemFieldValueDefinition.FieldId"));

                hasException = true;
            }

            Assert.IsTrue(hasException);
        }

        #endregion
    }
}
