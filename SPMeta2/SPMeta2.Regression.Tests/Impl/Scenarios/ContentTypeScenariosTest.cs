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
    public class ContentTypeScenariosTest : SPMeta2RegresionEventsTestBase
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

        #region tests

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes")]
        public void CanDeploy_CustomListItemContentType()
        {
            TestRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Item;
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.ContentTypes")]
        public void CanDeploy_CustomDocumentContentType()
        {
            TestRandomDefinition<ContentTypeDefinition>(def =>
            {
                def.ParentContentTypeId = BuiltInContentTypeId.Document;
            });
        }


        #endregion
    }
}
