using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;

namespace SPMeta2.Regression.Impl.Tests.ModelHandlers
{
    [TestClass]
    public class CSOMSandboxSolutionModelHandlerTests
    {
        #region constructors

        public CSOMSandboxSolutionModelHandlerTests()
        {
            Rnd = new DefaultRandomService();
        }

        #endregion

        #region static

        static CSOMSandboxSolutionModelHandlerTests()
        {
            RegressionService = new RegressionTestService();
            RegressionService.ModelGeneratorService.RegisterDefinitionGenerators(typeof(ImageRenditionDefinitionGenerator).Assembly);
        }

        #endregion

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

        public ModelGeneratorService ModelGeneratorService
        {
            get { return RegressionService.ModelGeneratorService; }
        }

        public static RegressionTestService RegressionService { get; set; }

        public RandomService Rnd { get; set; }

        #endregion

        #region

        [TestMethod]
        [TestCategory("Regression.Impl.CSOM.SandboxSolutionModelHandler")]
        public void ShouldNotDeploy_CSOM_SandboxSolution_WithDotsInFileName()
        {
            var hasException = false;

            var sandbox = ModelGeneratorService.GetRandomDefinition<SandboxSolutionDefinition>(def =>
            {
                def.Activate = true;
                def.SolutionId = Guid.NewGuid();
                def.FileName = string.Format("{0}.{1}.wsp", Rnd.String(4), Rnd.String(4));
            });

            try
            {
                var handler = new SPMeta2.CSOM.Standard.ModelHandlers.SandboxSolutionModelHandler();

                handler.DeployModel(null, sandbox);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(SPMeta2NotSupportedException));
                Assert.IsTrue(ex.Message.Contains("SandboxSolutionDefinition.FileName"));

                hasException = true;
            }

            Assert.IsTrue(hasException);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.CSOM.SandboxSolutionModelHandler")]
        public void ShouldNotDeploy_CSOM_SandboxSolution_WithEmptySolutionId()
        {
            var hasException = false;

            var sandbox = ModelGeneratorService.GetRandomDefinition<SandboxSolutionDefinition>(def =>
            {
                def.Activate = true;
                def.FileName = string.Format("{0}.wsp", Rnd.String());
                def.SolutionId = Guid.Empty;
            });

            try
            {
                var handler = new SPMeta2.CSOM.Standard.ModelHandlers.SandboxSolutionModelHandler();

                handler.DeployModel(null, sandbox);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(SPMeta2NotSupportedException));
                Assert.IsTrue(ex.Message.Contains("SandboxSolutionDefinition.SolutionId"));

                hasException = true;
            }

            Assert.IsTrue(hasException);
        }

        [TestMethod]
        [TestCategory("Regression.Impl.CSOM.SandboxSolutionModelHandler")]
        public void ShouldNotDeploy_CSOM_SandboxSolution_WithActivate_Eq_False()
        {
            var hasException = false;

            var sandbox = ModelGeneratorService.GetRandomDefinition<SandboxSolutionDefinition>(def =>
            {
                def.Activate = false;
                def.FileName = string.Format("{0}.wsp", Rnd.String());
                def.SolutionId = Guid.NewGuid();
            });

            try
            {
                var handler = new SPMeta2.CSOM.Standard.ModelHandlers.SandboxSolutionModelHandler();

                handler.DeployModel(null, sandbox);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(SPMeta2NotSupportedException));
                Assert.IsTrue(ex.Message.Contains("SandboxSolutionDefinition.Activate"));

                hasException = true;
            }

            Assert.IsTrue(hasException);
        }

        #endregion

    }
}
