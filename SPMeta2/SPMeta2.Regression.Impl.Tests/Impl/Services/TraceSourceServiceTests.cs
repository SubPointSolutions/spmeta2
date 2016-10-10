using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Services;
using System.Reflection;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.CSOM;
using SPMeta2.Containers.Services;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Containers.SSOM;
using SPMeta2.Regression.Impl.Tests.Impl.Services.Base;
using SPMeta2.SSOM.Services;
using SPMeta2.Services.Impl;
using SPMeta2.Diagnostic;
using SPMeta2.Containers.Services.Rnd;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{

    [TestClass]
    public class TraceSourceServiceTests
    {
        public TraceSourceServiceTests()
        {
            Service = new TraceSourceService();
            Rnd = new DefaultRandomService();
        }

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

        #region properties

        public TraceServiceBase Service { get; set; }
        public DefaultRandomService Rnd { get; set; }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.TraceSourceService")]
        [TestCategory("CI.Core")]
        public void Can_Create_TraceSourceService()
        {
            var service = new TraceSourceService();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.TraceSourceService")]
        [TestCategory("CI.Core")]
        public void TraceSourceService_Can_Verbose()
        {
            Service.Verbose(Rnd.Int(), null);
            Service.Verbose(Rnd.Int(), Rnd.String());

            Service.VerboseFormat(Rnd.Int(), "{0}{1}", new[] { Rnd.String(), Rnd.String() });
        }

        [TestMethod]
        [TestCategory("Regression.Impl.TraceSourceService")]
        [TestCategory("CI.Core")]
        public void TraceSourceService_Can_Information()
        {
            Service.Information(Rnd.Int(), null);
            Service.Information(Rnd.Int(), Rnd.String());

            Service.InformationFormat(Rnd.Int(), "{0}{1}", new[] { Rnd.String(), Rnd.String() });
        }

        [TestMethod]
        [TestCategory("Regression.Impl.TraceSourceService")]
        [TestCategory("CI.Core")]
        public void TraceSourceService_Can_Error()
        {
            Service.Error(Rnd.Int(), null);
            Service.Error(Rnd.Int(), Rnd.String());

            Service.ErrorFormat(Rnd.Int(), "{0}{1}", new[] { Rnd.String(), Rnd.String() });
        }

        [TestMethod]
        [TestCategory("Regression.Impl.TraceSourceService")]
        [TestCategory("CI.Core")]
        public void TraceSourceService_Can_Warning()
        {
            Service.Warning(Rnd.Int(), null);
            Service.Warning(Rnd.Int(), Rnd.String());

            Service.WarningFormat(Rnd.Int(), "{0}{1}", new[] { Rnd.String(), Rnd.String() });
        }

        #endregion
    }
}
