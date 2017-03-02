using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Common;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Regression.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.Services.Impl;
using SPMeta2.Syntax.Default;
using System.Collections.Generic;
using SPMeta2.Extensions;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class IncrementalModelTreeTraverseServiceTests
    {
        #region default

        [TestMethod]
        [TestCategory("Regression.Services.IncrementalModelTreeTraverseService")]
        [TestCategory("CI.Core")]
        public void Can_Create_IncrementalModelTreeTraverseService()
        {
            var service = new IncrementalModelTreeTraverseService();
        }

        #endregion

        #region non-singlentons

        [TestMethod]
        [TestCategory("Regression.Services.IncrementalModelTreeTraverseService.NonSingleton")]
        [TestCategory("CI.Core")]
        public void Incremental_Update_NonSingleton_ModelNodes_SameModels()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField();
                site.AddRandomField();
                site.AddRandomField();
            });

            var prevModel = model;
            var currentModel = model;

            var firstProvisionService = new FakeIncrementalModelTreeTraverseService();

            firstProvisionService.PreviousModelHash = new ModelHash();
            firstProvisionService.Traverse(null, currentModel);

            var trace = ServiceContainer.Instance.GetService<TraceServiceBase>();

            // check one more with second provision
            var secondProvisionService = new FakeIncrementalModelTreeTraverseService();

            secondProvisionService.PreviousModelHash = firstProvisionService.CurrentModelHash;
            secondProvisionService.Traverse(null, currentModel);

            // asserts
            // should be NONE of the nodes to update on the same model
            Assert.AreEqual(0, secondProvisionService.ModelNodesToUpdate.Count);
            Assert.AreEqual(GetTotalModelNodeCount(model), secondProvisionService.ModelNodesToSkip.Count);
        }

        [TestMethod]
        [TestCategory("Regression.Services.IncrementalModelTreeTraverseService.NonSingleton")]
        [TestCategory("CI.Core")]
        public void Incremental_Update_NonSingleton_ModelNodes_PlusRandomModelNodes()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField();
                site.AddRandomField();
                site.AddRandomField();
            });

            var prevModel = model;
            var currentModel = model;

            var firstProvisionService = new FakeIncrementalModelTreeTraverseService();

            firstProvisionService.PreviousModelHash = new ModelHash();
            firstProvisionService.Traverse(null, currentModel);

            var trace = ServiceContainer.Instance.GetService<TraceServiceBase>();

            ModelNode newField1 = null;
            ModelNode newField2 = null;

            // newnodes to add
            model.AddRandomField(f => { newField1 = f; });
            model.AddRandomField(f => { newField2 = f; });

            // check one more with second provision
            var secondProvisionService = new FakeIncrementalModelTreeTraverseService();

            secondProvisionService.PreviousModelHash = firstProvisionService.CurrentModelHash;
            secondProvisionService.Traverse(null, currentModel);

            // asserts
            // should be 2 of the nodes to update on the same model
            Assert.AreEqual(2, secondProvisionService.ModelNodesToUpdate.Count);
            Assert.AreEqual(GetTotalModelNodeCount(model),
                            secondProvisionService.ModelNodesToSkip.Count + secondProvisionService.ModelNodesToUpdate.Count);

            // new field must be marked as to be updated
            Assert.IsTrue(newField1.Options.RequireSelfProcessing);
            Assert.IsTrue(newField2.Options.RequireSelfProcessing);
        }

        #endregion

        #region utils

        public int GetTotalModelNodeCount(ModelNode model)
        {
            if (model == null)
                return 0;

            var result = 0;

            model.WithNodesOfType<DefinitionBase>(n => { result++; });

            return result;
        }

        #endregion
    }

    public class FakeIncrementalModelTreeTraverseService : IncrementalModelTreeTraverseService
    {
        #region constructors
        public FakeIncrementalModelTreeTraverseService()
        {
            this.OnModelHandlerResolve += (node =>
            {
                return new EmptyModelhandler();
            });

            ModelNodesToUpdate = new List<ModelNode>();
            ModelNodesToSkip = new List<ModelNode>();
        }
        #endregion

        #region properties

        public List<ModelNode> ModelNodesToUpdate { get; set; }
        public List<ModelNode> ModelNodesToSkip { get; set; }

        #endregion

        protected override void OnBeforeDeployModel(object modelHost, ModelNode modelNode)
        {
            // process deployment need
            base.OnBeforeDeployModel(modelHost, modelNode);

            if (modelNode.Options.RequireSelfProcessing)
                ModelNodesToUpdate.Add(modelNode);
            else
                ModelNodesToSkip.Add(modelNode);
        }
    }
}
