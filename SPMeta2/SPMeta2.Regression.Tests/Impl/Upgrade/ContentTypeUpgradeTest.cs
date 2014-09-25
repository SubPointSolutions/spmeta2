using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Impl.Upgrade
{
    [TestClass]
    public class ContentTypeUpgradeTest : SPMeta2RegresionEventsTestBase
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

        #region default

        // deploy, add to lists, create content and upgrade

        
      
        
        #endregion
    }
}
