using System;
using System.Linq;
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
using SPMeta2.Regression.Utils;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.Attributes;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Impl.Services
{
   

    [TestClass]
    public class IncrementalModelTreeTraverseServiceTests
    {
        #region constructors

        public IncrementalModelTreeTraverseServiceTests()
        {
            ModelPrintService = new DefaultIncrementalModelPrettyPrintService();

            Rnd = new DefaultRandomService();

            ModelGeneratorService = new ModelGeneratorService();
            ModelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);
        }

        #endregion

        #region properties

        public ModelPrintServiceBase ModelPrintService { get; set; }

        public ModelGeneratorService ModelGeneratorService { get; set; }

        public DefaultRandomService Rnd { get; set; }

        #endregion

        #region default

        [TestMethod]
        [TestCategory("Regression.Services.IncrementalModelTreeTraverseService")]
        [TestCategory("CI.Core")]
        public void Can_Create_IncrementalModelTreeTraverseService()
        {
            var service = new DefaultIncrementalModelTreeTraverseService();

        }



        #endregion

        #region switching the mode

        [TestMethod]
        [TestCategory("Regression.Services.ProvisionServiceBase")]
        [TestCategory("CI.Core")]
        public void Can_Toggle_Incremental_ProvisionServiceMode()
        {
            var service = new ProvisionServiceBase();

            // normal mode
            Assert.AreEqual(typeof(DefaultModelTreeTraverseService), service.ModelTraverseService.GetType());

            // incremental provision
            service.SetIncrementalProvisionMode();
            Assert.AreEqual(typeof(DefaultIncrementalModelTreeTraverseService), service.ModelTraverseService.GetType());

            // switching back to normal
            service.SetDefaultProvisionMode();
            Assert.AreEqual(typeof(DefaultModelTreeTraverseService), service.ModelTraverseService.GetType());

            // incremental provision
            service.SetIncrementalProvisionMode();
            Assert.AreEqual(typeof(DefaultIncrementalModelTreeTraverseService), service.ModelTraverseService.GetType());

            // switching back to normal
            service.SetDefaultProvisionMode();
            Assert.AreEqual(typeof(DefaultModelTreeTraverseService), service.ModelTraverseService.GetType());
        }

        [TestMethod]
        [TestCategory("Regression.Services.ProvisionServiceBase")]
        [TestCategory("CI.Core")]
        public void Can_Reset_ProvisionServiceMode()
        {
            var service = new ProvisionServiceBase();

            // normal mode
            Assert.AreEqual(typeof(DefaultModelTreeTraverseService), service.ModelTraverseService.GetType());

            // incremental provision
            service.SetIncrementalProvisionMode();
            Assert.AreEqual(typeof(DefaultIncrementalModelTreeTraverseService), service.ModelTraverseService.GetType());

            // switching back to normal
            service.ResetProvisionMode();
            Assert.AreEqual(typeof(DefaultModelTreeTraverseService), service.ModelTraverseService.GetType());
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

            RegressionUtils.WriteLine("Original model:");
            RegressionUtils.WriteLine(ModelPrintService.PrintModel(currentModel));

            secondProvisionService.PreviousModelHash = firstProvisionService.CurrentModelHash;
            secondProvisionService.Traverse(null, currentModel);

            RegressionUtils.WriteLine(string.Empty);
            RegressionUtils.WriteLine("Provisioned model:");
            RegressionUtils.WriteLine(ModelPrintService.PrintModel(currentModel));

            // asserts
            // should be NONE of the nodes to update on the same model
            Assert.AreEqual(0, secondProvisionService.ModelNodesToUpdate.Count);
            Assert.AreEqual(GetTotalModelNodeCount(model), secondProvisionService.ModelNodesToSkip.Count);
        }

        [TestMethod]
        [TestCategory("Regression.Services.IncrementalModelTreeTraverseService.Random")]
        [TestCategory("CI.Core")]
        public void Incremental_Update_AllDefinitions_As_RandomModels()
        {
            var spMetaAssembly = typeof(FieldDefinition).Assembly;
            var spMetaStandardAssembly = typeof(TaxonomyFieldDefinition).Assembly;

            var allDefinitionTypes = ReflectionUtils.GetTypesFromAssemblies<DefinitionBase>(new[]
            {
                spMetaAssembly,
                spMetaStandardAssembly
            }).OrderBy(t => t.Name);

            var allDefinitionTypesCount = allDefinitionTypes.Count();
            var currentDefinitionTypeIndex = 0;

            foreach (var definitionType in allDefinitionTypes)
            {
                currentDefinitionTypeIndex++;
                RegressionUtils.WriteLine(string.Format("[{0}/{1}] Testing definition:[{2}]",
                    new object[] {
                        currentDefinitionTypeIndex,
                        allDefinitionTypesCount,
                        definitionType.FullName
                    }));

                var model = GetVeryRandomModel(definitionType, SPObjectModelType.CSOM);

                var prevModel = model;
                var currentModel = model;

                var firstProvisionService = new FakeIncrementalModelTreeTraverseService();

                firstProvisionService.PreviousModelHash = new ModelHash();
                firstProvisionService.Traverse(null, currentModel);

                var trace = ServiceContainer.Instance.GetService<TraceServiceBase>();

                // check one more with second provision
                var secondProvisionService = new FakeIncrementalModelTreeTraverseService();

                RegressionUtils.WriteLine("Original model:");
                RegressionUtils.WriteLine(ModelPrintService.PrintModel(currentModel));

                secondProvisionService.PreviousModelHash = firstProvisionService.CurrentModelHash;
                secondProvisionService.Traverse(null, currentModel);

                RegressionUtils.WriteLine(string.Empty);
                RegressionUtils.WriteLine("Provisioned model:");
                RegressionUtils.WriteLine(ModelPrintService.PrintModel(currentModel));

                // asserts
                var expectedProvisionNodesCount = 0;

                expectedProvisionNodesCount += secondProvisionService.GetTotalSingleIdentityNodeCount();
                expectedProvisionNodesCount += secondProvisionService.GetTotalIgnoredNodeCount();

                Assert.AreEqual(expectedProvisionNodesCount, secondProvisionService.ModelNodesToUpdate.Count);
                Assert.AreEqual(GetTotalModelNodeCount(model) - expectedProvisionNodesCount, secondProvisionService.ModelNodesToSkip.Count);
            }
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

            RegressionUtils.WriteLine("Original model:");
            RegressionUtils.WriteLine(ModelPrintService.PrintModel(currentModel));

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

            RegressionUtils.WriteLine(string.Empty);
            RegressionUtils.WriteLine("Provisioned model:");
            RegressionUtils.WriteLine(ModelPrintService.PrintModel(currentModel));

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

        protected ModelNode GetVeryRandomModel(Type definitionType, SPObjectModelType spObjectModelType)
        {
            var definitionSandbox = ModelGeneratorService.GenerateModelTreeForDefinition(definitionType, spObjectModelType);
            var additionalDefinitions = ModelGeneratorService.GetAdditionalDefinitions(definitionType);

            ModelGeneratorService.ComposeModelWithAdditionalDefinitions(definitionSandbox, additionalDefinitions, spObjectModelType);

            // more random definitions
            var messyDefinitions = new List<DefinitionBase>();

            var flatNodes = definitionSandbox.Flatten().ToArray();

            foreach (var node in flatNodes)
            {
                if (node.ChildModels.Count == 0)
                    continue;

                var defType = node.ChildModels[0].Value.GetType();
                var randomFactor = Rnd.Int(5) + 3;

                for (var i = 0; i < randomFactor; i++)
                {
                    node.AddDefinitionNode(ModelGeneratorService.GetRandomDefinition(defType));
                }
            }

            return definitionSandbox;
        }

        protected int GetTotalModelNodeCount(ModelNode model)
        {
            if (model == null)
                return 0;

            var result = 0;

            model.WithNodesOfType<DefinitionBase>(n => { result++; });

            return result;
        }

        #endregion
    }

    public class FakeIncrementalModelTreeTraverseService : DefaultIncrementalModelTreeTraverseService
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

        protected override void OnBeforeDeployModelNode(object modelHost, ModelNode modelNode)
        {
            // process deployment need
            base.OnBeforeDeployModelNode(modelHost, modelNode);

            if (modelNode.Options.RequireSelfProcessing)
                ModelNodesToUpdate.Add(modelNode);
            else
                ModelNodesToSkip.Add(modelNode);
        }

        public int GetTotalSingleIdentityNodeCount()
        {
            // && RequireSelfProcessing cause root defs such as FarmDefinition must be excluded
            // getting only singletons from the actual model, not the root nodes
            var result =
                ModelNodesToUpdate.Count(n => IsSingletonIdentityDefinition(n.Value) && n.Options.RequireSelfProcessing)
                + ModelNodesToSkip.Count(n => IsSingletonIdentityDefinition(n.Value) && n.Options.RequireSelfProcessing);

            return result;
        }

        public int GetTotalIgnoredNodeCount()
        {
            return IgnoredModelNodes.Count();
        }
    }
}
