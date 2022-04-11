using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services.Impl;
using SPMeta2.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services.CSOM
{

    public class MockedRequireCSOMRuntimeVersionDeploymentService : RequireCSOMRuntimeVersionDeploymentService
    {
        public MockedRequireCSOMRuntimeVersionDeploymentService(Version version)
        {
            this.Version = version;
        }


        public Version Version { get; set; }

        protected override Version GetCSOMRuntimeVersion()
        {
            return Version;
        }
    }

    [TestClass]
    public class RequireCSOMRuntimeVersionDeploymentServiceTests
    {
        #region constructors

        public RequireCSOMRuntimeVersionDeploymentServiceTests()
        {
            Service = new RequireCSOMRuntimeVersionDeploymentService();
        }

        #endregion

        #region properties

        public RequireCSOMRuntimeVersionDeploymentService Service { get; set; }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.RequireCSOMRuntimeVersionDeploymentService")]
        [TestCategory("CI.Core")]
        public void RequireCSOMRuntimeVersionDeploymentService_Ignore2010()
        {
            var service = new MockedRequireCSOMRuntimeVersionDeploymentService(new Version("14.0.4762.1000"));

            // must not throw exeption
            service.DeployModel(null, null);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.RequireCSOMRuntimeVersionDeploymentService")]
        [TestCategory("CI.Core")]
        public void RequireCSOMRuntimeVersionDeploymentService_Pass2013_Minimum()
        {
            var service = new MockedRequireCSOMRuntimeVersionDeploymentService(RequireCSOMRuntimeVersionDeploymentService.MinimalVersion);

            // must not throw exeption
            service.DeployModel(null, null);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.RequireCSOMRuntimeVersionDeploymentService")]
        [TestCategory("CI.Core")]
        public void RequireCSOMRuntimeVersionDeploymentService_Pass2013_Minimum_Higher()
        {
            var service = new MockedRequireCSOMRuntimeVersionDeploymentService(new Version(
                    RequireCSOMRuntimeVersionDeploymentService.MinimalVersion.Major,
                    RequireCSOMRuntimeVersionDeploymentService.MinimalVersion.Minor,
                    RequireCSOMRuntimeVersionDeploymentService.MinimalVersion.Build + 1,
                    RequireCSOMRuntimeVersionDeploymentService.MinimalVersion.Revision));

            // must not throw exeption
            service.DeployModel(null, null);
        }



        [TestMethod]
        [TestCategory("Regression.Impl.RequireCSOMRuntimeVersionDeploymentService")]
        [TestCategory("CI.Core")]
        [ExpectedException(typeof(SPMeta2NotSupportedException))]
        public void RequireCSOMRuntimeVersionDeploymentService_Pass2013_Minimum_Lower()
        {
            var service = new MockedRequireCSOMRuntimeVersionDeploymentService(new Version(
                   RequireCSOMRuntimeVersionDeploymentService.MinimalVersion.Major,
                   RequireCSOMRuntimeVersionDeploymentService.MinimalVersion.Minor,
                   RequireCSOMRuntimeVersionDeploymentService.MinimalVersion.Build - 1,
                   RequireCSOMRuntimeVersionDeploymentService.MinimalVersion.Revision));

            // must throw exeption
            service.DeployModel(null, null);
        }

        #endregion
    }
}
