using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Containers.Standard.DefinitionGenerators;
using SPMeta2.CSOM.Standard.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Impl.Tests.ModelHandlers.Base;
using SPMeta2.CSOM.Services;
using SPMeta2.CSOM.ModelHosts;
using Microsoft.SharePoint.Client;
using SPMeta2.Enumerations;
using SPMeta2.Extensions;

using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Impl.Tests.ModelHandlers
{
    public static class SampleConsts
    {
        public static string DefaultMetadataGroup = "sd";
    }

    [TestClass]
    public class CSOMSandboxSolutionModelHandlerTests : ModelHandlerTestBase
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

        #region tests



        public static class FieldModels
        {
            public static FieldDefinition Contact = new FieldDefinition
            {
                Id = new Guid("1d20b513-0095-4735-a68d-c5c972494afc"),
                Title = "Client ID",
                InternalName = "clnt_ClientId",
                Group = SampleConsts.DefaultMetadataGroup,
                FieldType = BuiltInFieldTypes.Text
            };

            public static FieldDefinition Details = new FieldDefinition
            {
                Id = new Guid("2a121dbf-ad68-4f2c-af49-f8671dfd4bf7"),
                Title = "Client Name",
                InternalName = "clnt_ClientName",
                Group = SampleConsts.DefaultMetadataGroup,
                FieldType = BuiltInFieldTypes.Text
            };

        }

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
