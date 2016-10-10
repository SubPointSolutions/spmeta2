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
using SPMeta2.Models;
using SPMeta2.Definitions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Impl.Tests.Impl.Services
{

    [TestClass]
    public class DefaultModelStatInfoServiceTests
    {
        public DefaultModelStatInfoServiceTests()
        {
            Service = new DefaultModelStatInfoService();
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

        public DefaultModelStatInfoService Service { get; set; }
        public DefaultRandomService Rnd { get; set; }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.DefaultModelStatInfoService")]
        [TestCategory("CI.Core")]
        public void Can_Get_ModelStatInfoServiceBase_Instance()
        {
            var service = ServiceContainer.Instance.GetService<ModelStatInfoServiceBase>();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.DefaultModelStatInfoService")]
        [TestCategory("CI.Core")]
        public void Can_Create_DefaultModelStatInfoService()
        {
            var service = new DefaultModelStatInfoService();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.DefaultModelStatInfoService")]
        [TestCategory("CI.Core")]
        public void DefaultModelStatInfoService_GetModelStat()
        {
            Service.GetModelStat(new ModelNode { Value = new FieldDefinition { } });
            Service.GetModelStat(new ModelNode { Value = new FieldDefinition { } }, new ModelStatInfoServiceOptions());
        }

        [TestMethod]
        [TestCategory("Regression.Impl.DefaultModelStatInfoService")]
        [TestCategory("CI.Core")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DefaultModelStatInfoService_GetModelStat_ArgumentNullException()
        {
            Service.GetModelStat(new ModelNode { Value = new FieldDefinition { } }, null);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.DefaultModelStatInfoService")]
        [TestCategory("CI.Core")]
        public void DefaultModelStatInfoService_GetModelStat_1()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddField(new FieldDefinition());
            });

            var result = Service.GetModelStat(model);

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.TotalModelNodeCount);
            Assert.AreEqual(2, result.NodeStat.Count);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.DefaultModelStatInfoService")]
        [TestCategory("CI.Core")]
        public void DefaultModelStatInfoService_GetModelStat_2()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddField(new FieldDefinition());
                web.AddField(new FieldDefinition());

                web.AddList(new ListDefinition());
            });

            var result = Service.GetModelStat(model);

            Assert.IsNotNull(result);

            Assert.AreEqual(4, result.TotalModelNodeCount);
            Assert.AreEqual(3, result.NodeStat.Count);
        }

        #endregion
    }
}
