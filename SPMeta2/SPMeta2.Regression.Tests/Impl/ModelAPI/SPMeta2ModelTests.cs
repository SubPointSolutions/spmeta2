using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;
using SPMeta2.Containers.Utils;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Extensions;

namespace SPMeta2.Regression.Tests.Impl.ModelAPI
{
    [TestClass]
    public class SPMeta2ModelDeploymentTests : SPMeta2ProvisionRegresionTestBase
    {
        public SPMeta2ModelDeploymentTests()
        {

        }

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

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_FarmModel()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var model = SPMeta2Model.NewFarmModel(m => { });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_WebApplicationModel()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var model = SPMeta2Model.NewWebApplicationModel(m => { });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_SiteModel()
        {
            var model = SPMeta2Model.NewSiteModel(m => { });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_WebModel()
        {
            var model = SPMeta2Model.NewWebModel(m => { });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_ListModel()
        {
            var model = SPMeta2Model.NewListModel(m => { });

            TestModel(model);
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model")]
        public void CanDeploy_ListModel_WithFolders()
        {
            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSiteFeature(BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(def =>
                {
                    def.Enable = true;
                }));
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddWebFeature(BuiltInWebFeatures.SharePointServerPublishing.Inherit(def =>
                {
                    def.Enable = true;
                }));
            });

            var model = SPMeta2Model.NewListModel(list =>
            {
                list.AddFolder(ModelGeneratorService.GetRandomDefinition<FolderDefinition>());
                list.AddFolder(ModelGeneratorService.GetRandomDefinition<FolderDefinition>());
                list.AddFolder(ModelGeneratorService.GetRandomDefinition<FolderDefinition>());
            });

            TestModels(new ModelNode[]{
                siteModel,
                webModel,
                model
            });
        }

        #endregion
    }

    [TestClass]
    public class SPMeta2ModelTests : SPMeta2DefinitionRegresionTestBase
    {


        #region serialization

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.Serialization")]
        [TestCategory("CI.Core")]
        public void CanSerialize_SiteModelToXMLAndBack()
        {
            var orginalModel = SPMeta2Model.NewSiteModel(site =>
            {

            });

            IndentableTrace.WithScope(trace =>
            {
                var modelString = SPMeta2Model.ToXML(orginalModel);
                Assert.IsFalse(string.IsNullOrEmpty(modelString));

                trace.WriteLine("XML");
                trace.WriteLine(modelString);

                var deserializedModel = SPMeta2Model.FromXML(modelString);
                Assert.IsNotNull(deserializedModel);

            });
        }


        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.Serialization")]
        [TestCategory("CI.Core")]
        public void CanSerialize_SiteModelToJSONAndBack()
        {
            var orginalModel = SPMeta2Model.NewSiteModel(site =>
            {

            });

            IndentableTrace.WithScope(trace =>
            {
                var modelString = SPMeta2Model.ToJSON(orginalModel);
                Assert.IsFalse(string.IsNullOrEmpty(modelString));

                trace.WriteLine("JSON");
                trace.WriteLine(modelString);

                var deserializedModel = SPMeta2Model.FromJSON(modelString);
                Assert.IsNotNull(deserializedModel);
            });
        }

        #endregion

        #region compatibility

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.Compatibility")]
        [TestCategory("CI.Core")]
        public void Should_Pass_On_Valid_SSOM_CSOM()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(ModelGeneratorService.GetRandomDefinition<FieldDefinition>());
            });

            // both CSOM / SSOM
            Assert.IsTrue(SPMeta2Model.IsCSOMCompatible(model));
            Assert.IsTrue(SPMeta2Model.IsSSOMCompatible(model));

            Assert.IsTrue(model.IsCSOMCompatible());
            Assert.IsTrue(model.IsSSOMCompatible());
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.Compatibility")]
        [TestCategory("CI.Core")]
        public void Should_Pass_On_Valid_SSOM_Invalid_CSOM()
        {
            var model = SPMeta2Model.NewWebApplicationModel(webApp =>
            {
                webApp.AddSite(ModelGeneratorService.GetRandomDefinition<SiteDefinition>());
            });

            // - CSOM / + SSOM
            Assert.IsFalse(SPMeta2Model.IsCSOMCompatible(model));
            Assert.IsTrue(SPMeta2Model.IsSSOMCompatible(model));

            Assert.IsFalse(model.IsCSOMCompatible());
            Assert.IsTrue(model.IsSSOMCompatible());
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.Compatibility")]
        [TestCategory("CI.Core")]
        public void Should_Pass_On_Valid_SSOM_Invalid_CSOM_2()
        {
            var model = SPMeta2Model.NewFarmModel(farm =>
            {
                farm.AddProperty(ModelGeneratorService.GetRandomDefinition<PropertyDefinition>());
            });

            // - CSOM / + SSOM
            Assert.IsFalse(SPMeta2Model.IsCSOMCompatible(model));
            Assert.IsTrue(SPMeta2Model.IsSSOMCompatible(model));

            Assert.IsFalse(model.IsCSOMCompatible());
            Assert.IsTrue(model.IsSSOMCompatible());
        }

        #endregion

        #region new model API

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.NewXXXModel")]
        [TestCategory("CI.Core")]
        public void SPMeta2Model_NewFarmModel_Contract()
        {
            var expectedType = typeof(FarmModelNode);
            var newDefinition = new FarmDefinition();

            // new
            Assert.IsTrue(SPMeta2Model.NewFarmModel().GetType() == expectedType);

            // new with callback
            Assert.IsTrue(SPMeta2Model.NewFarmModel(model => { }).GetType() == expectedType);

            // new definition
            Assert.IsTrue(SPMeta2Model.NewFarmModel(newDefinition).GetType() == expectedType);

            // new definition with callback
            Assert.IsTrue(SPMeta2Model.NewFarmModel(newDefinition, farm => { }).GetType() == expectedType);
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.NewXXXModel")]
        [TestCategory("CI.Core")]
        public void SPMeta2Model_NewWebAppModel_Contract()
        {
            var expectedType = typeof(WebApplicationModelNode);
            var newDefinition = new WebApplicationDefinition();

            // new
            Assert.IsTrue(SPMeta2Model.NewWebApplicationModel().GetType() == expectedType);

            // new with callback
            Assert.IsTrue(SPMeta2Model.NewWebApplicationModel(model => { }).GetType() == expectedType);

            // new definition
            Assert.IsTrue(SPMeta2Model.NewWebApplicationModel(newDefinition).GetType() == expectedType);

            // new definition with callback
            Assert.IsTrue(SPMeta2Model.NewWebApplicationModel(newDefinition, farm => { }).GetType() == expectedType);
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.NewXXXModel")]
        [TestCategory("CI.Core")]
        public void SPMeta2Model_NewSiteModel_Contract()
        {
            var expectedType = typeof(SiteModelNode);
            var newDefinition = new SiteDefinition();

            // new
            Assert.IsTrue(SPMeta2Model.NewSiteModel().GetType() == expectedType);

            // new with callback
            Assert.IsTrue(SPMeta2Model.NewSiteModel(model => { }).GetType() == expectedType);

            // new definition
            Assert.IsTrue(SPMeta2Model.NewSiteModel(newDefinition).GetType() == expectedType);

            // new definition with callback
            Assert.IsTrue(SPMeta2Model.NewSiteModel(newDefinition, node => { }).GetType() == expectedType);
        }


        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.NewXXXModel")]
        [TestCategory("CI.Core")]
        public void SPMeta2Model_NewWebModel_Contract()
        {
            var expectedType = typeof(WebModelNode);
            var newDefinition = new WebDefinition();

            // new
            Assert.IsTrue(SPMeta2Model.NewWebModel().GetType() == expectedType);

            // new with callback
            Assert.IsTrue(SPMeta2Model.NewWebModel(model => { }).GetType() == expectedType);

            // new definition
            Assert.IsTrue(SPMeta2Model.NewWebModel(newDefinition).GetType() == expectedType);

            // new definition with callback
            Assert.IsTrue(SPMeta2Model.NewWebModel(newDefinition, node => { }).GetType() == expectedType);
        }

        [TestMethod]
        [TestCategory("Regression.SPMeta2Model.NewXXXModel")]
        [TestCategory("CI.Core")]
        public void SPMeta2Model_NewListModel_Contract()
        {
            var expectedType = typeof(ListModelNode);
            var newDefinition = new ListDefinition();

            // new
            Assert.IsTrue(SPMeta2Model.NewListModel().GetType() == expectedType);

            // new with callback
            Assert.IsTrue(SPMeta2Model.NewListModel(model => { }).GetType() == expectedType);

            // new definition
            Assert.IsTrue(SPMeta2Model.NewListModel(newDefinition).GetType() == expectedType);

            // new definition with callback
            Assert.IsTrue(SPMeta2Model.NewListModel(newDefinition, node => { }).GetType() == expectedType);
        }

        #endregion
    }
}
