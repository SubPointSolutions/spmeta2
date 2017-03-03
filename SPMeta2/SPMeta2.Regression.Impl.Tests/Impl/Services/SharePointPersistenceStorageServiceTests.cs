using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Common;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.Standard.Services;
using SPMeta2.Interfaces;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.Services;
using SPMeta2.SSOM.Standard.Services;
using SPMeta2.Utils;
using SPMeta2.Containers.CSOM;
using SPMeta2.Services.Impl;
using SPMeta2.Syntax.Default;
using SPMeta2.Containers.O365;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using System;
using SPMeta2.Containers.SSOM;
using SPMeta2.CSOM.Services.Impl;
using SPMeta2.Extensions;
using SPMeta2.SSOM.Services.Impl;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{
    [TestClass]
    public class SharePointPersistenceStorageServiceTests
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

        #region test

        [TestMethod]
        [TestCategory("Regression.Impl.SharePointPersistenceStorageServiceTests")]
        [TestCategory("CI.Core")]
        public void Can_SharePointPersistenceStorageServices()
        {
            Assert.IsNotNull(new DefaultCSOMWebPropertyBagStorage());
            
            Assert.IsNotNull(new DefaultSSOMFarmPropertyBagStorage());
            Assert.IsNotNull(new DefaultSSOMWebApplicationPropertyBagStorage());
            Assert.IsNotNull(new DefaultSSOMWebPropertyBagStorage());
        }

        #endregion
    }
}
