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

        public static class ContentTypeModels
        {
            public static ContentTypeDefinition CustomItem = new ContentTypeDefinition
            {
                Id = new Guid("ccf280ad-5e90-43a0-ba7e-278a62a13e76"),
                Name = "Customer Document",
                ParentContentTypeId = BuiltInContentTypeId.Document,
                Group = SampleConsts.DefaultMetadataGroup
            };

            public static ContentTypeDefinition CustomDocument = new ContentTypeDefinition
            {
                Id = new Guid("a3dba0e0-ac48-4428-88c8-6ca121824172"),
                Name = "Customer Contract",
                ParentContentTypeId = BuiltInContentTypeId.Document,
                Group = SampleConsts.DefaultMetadataGroup
            };
        }

        [TestMethod]
        [TestCategory("Regression.Impl.CSOM.SandboxSolutionModelHandler")]
        public void SiteModel()
        {
            var targetSite = new Uri("http://tesla-dev:31415");

            var siteModel = SPMeta2Model
                   .NewSiteModel(site =>
                   {
                       site
                           .WithFields(fields =>
                           {
                               fields
                               .AddField(FieldModels.Contact)
                               .AddField(FieldModels.Details);
                           })
                           .WithContentTypes(contentTypes =>
                           {
                               contentTypes
                               .AddContentType(ContentTypeModels.CustomItem)
                               .AddContentType(ContentTypeModels.CustomDocument);
                           });
                   });

            using (var context = new ClientContext(targetSite))
            {
                var povisionService = new CSOMProvisionService();
                povisionService.DeployModel(SiteModelHost.FromClientContext(context), siteModel);
            }
        }

        public static class ListModels
        {
                public static ListDefinition TestLibrary = new ListDefinition
                {
                    Title = "Customer Documents",
                    Url = "CustomerDocs",
                    Description = "Stores customer related documents.",
                    TemplateType = BuiltInListTemplateTypeId.DocumentLibrary
                };

                public static ListDefinition TestList = new ListDefinition
                {
                    Title = "Customer Tasks",
                    Url = "CustomerTasks",
                    TemplateType = BuiltInListTemplateTypeId.TasksWithTimelineAndHierarchy
                };

                public static ListDefinition TestLinksList = new ListDefinition
                {
                    Title = "Customer Issues",
                    Url = "CustomerIssues",
                    TemplateType = BuiltInListTemplateTypeId.IssueTracking
                };

                public static ListDefinition KPI = new ListDefinition
                {
                    Title = "KPI History",
                    Url = "KPI",
                    TemplateType = BuiltInListTemplateTypeId.GenericList
                };
        }

        [TestMethod]
        [TestCategory("Regression.Impl.CSOM.SandboxSolutionModelHandler")]
        public void WebModel()
        {
            var targetSite = new Uri("http://tesla-dev:31415");

            var webModel = SPMeta2Model
                   .NewWebModel(web =>
                   {
                       web
                           .WithLists(lists =>
                           {
                               lists
                                   .AddList(ListModels.TestLibrary)
                                   .AddList(ListModels.TestList)
                                   .AddList(ListModels.TestLinksList);
                           });
                   });

            using (var context = new ClientContext(targetSite))
            {
                var povisionService = new CSOMProvisionService();
                povisionService.DeployModel(WebModelHost.FromClientContext(context), webModel);
            }
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
