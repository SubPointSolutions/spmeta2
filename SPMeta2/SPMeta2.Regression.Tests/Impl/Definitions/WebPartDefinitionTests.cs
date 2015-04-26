using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class WebPartDefinitionTests : SPMeta2RegresionScenarioTestBase
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
        [TestCategory("Regression.Definitions.WebPartDefinitionTests")]
        [ExpectedException(typeof(SPMeta2InvalidDefinitionPropertyException))]
        public void WebPartDefinitionTests_WebpartType_NonAssemblyQualifiedName_ShouldFail()
        {
            var type = "Microsoft.SharePoint.Publishing.WebControls.ContentByQueryWebPart, Microsoft.SharePoint.Publishing, Version=16.0.0.0, Culture=neutral";

            var def = new WebPartDefinition
            {
                WebpartType = type
            };
        }

        [TestMethod]
        [TestCategory("Regression.Definitions.WebPartDefinitionTests")]
        public void WebPartDefinitionTests_WebpartType_AssemblyQualifiedName_ShouldPass()
        {
            var type = "Microsoft.SharePoint.Publishing.WebControls.ContentByQueryWebPart, Microsoft.SharePoint.Publishing, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c";

            var def = new WebPartDefinition
            {
                WebpartType = type
            };

            Assert.IsTrue(type == def.WebpartType);
        }

        #endregion
    }
}
