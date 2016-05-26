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

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{

    [TestClass]
    public class DefaultDiagnosticInfoServiceTests
    {
        public DefaultDiagnosticInfoServiceTests()
        {
            Service = new DefaultDiagnosticInfoService();
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

        public DefaultDiagnosticInfoService Service { get; set; }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.DefaultDiagnosticInfoService")]
        [TestCategory("CI.Core.SharePoint")]
        public void DefaultDiagnosticInfoService_ShouldHaveAllProperties()
        {
            WarmupSharePoint();

            var info = Service.GetDiagnosticInfo();

            info.ToString();
            ValidateDiagnosticInfo(info);
        }



        [TestMethod]
        [TestCategory("Regression.Impl.DefaultDiagnosticInfoService")]
        [TestCategory("CI.Core.SharePoint")]
        public void DefaultDiagnosticInfoService_SPMeta2Diagnostic_NotNull()
        {
            WarmupSharePoint();

            var info = SPMeta2Diagnostic.GetDiagnosticInfo();

            info.ToString();
            ValidateDiagnosticInfo(info);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.DefaultDiagnosticInfoService")]
        [TestCategory("CI.Core")]
        public void DefaultDiagnosticInfoService_ShouldNotFail_WithoutSharePoint()
        {
            // should not fail on non-loaded SharePoint
            //WarmupSharePoint();

            var info = SPMeta2Diagnostic.GetDiagnosticInfo();
            info.ToString();
        }

        #endregion

        #region utils

        private static void ValidateDiagnosticInfo(DiagnosticInfo info)
        {
            Assert.IsNotNull(info);

            Assert.IsNotNull(info.SPMeta2FileLocation);
            Assert.IsNotNull(info.SPMeta2FileVersion);
            Assert.IsNotNull(info.SPMeta2ProductVersion);

            Assert.IsTrue(info.IsSSOMDetected);
            Assert.IsNotNull(info.SSOMFileLocation);
            Assert.IsNotNull(info.SSOMFileVersion);
            Assert.IsNotNull(info.SSOMProductVersion);

            Assert.IsTrue(info.IsCSOMDetected);
            Assert.IsNotNull(info.CSOMFileLocation);
            Assert.IsNotNull(info.CSOMFileVersion);
            Assert.IsNotNull(info.CSOMProductVersion);
        }

        private static void WarmupSharePoint()
        {
            // expecting that .GetDiagnosticInfo() will be called somewhere in try-cath
            // after some sharepoint relatd stuff was done (so assemblies were loaded to the current domain)
            var x = SPField.ListItemMenuState.Allowed;
            var y = new ListCreationInformation();
        }

        #endregion
    }
}
