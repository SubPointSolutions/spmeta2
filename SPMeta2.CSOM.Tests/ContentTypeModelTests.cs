using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Tests
{
    //[TestClass]

    public class ContentTypeModelTests : ClientOMSharePointTestBase
    {
        #region resources

        [TestInitialize]
        public void Setup()
        {
            // it is a good place to change TestSetting
            InitTestSettings();
        }

        [TestCleanup]
        public void Cleanup()
        {
            CleanupResources();
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeployResourceFilesToContentType()
        {
            var model = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                .WithContentTypes(contentTypes =>
                {
                    contentTypes
                        .AddContentType(ContentTypeModels.CustomItem, contentType =>
                        {
                            contentType
                                .AddModuleFile(ModuleFiles.JQuery);
                        });
                });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(SiteModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeployContentTypeModel()
        {
            var model = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                                        .WithFields(fields =>
                                        {
                                            fields
                                                .AddField(FieldModels.Contact)
                                                .AddField(FieldModels.Details)
                                                .AddField(FieldModels.Details1)
                                                .AddField(FieldModels.Details2);
                                        })
                                        .WithContentTypes(contentTypes =>
                                        {
                                            contentTypes
                                                .AddContentType(ContentTypeModels.CustomItem, contentType =>
                                                {
                                                    contentType
                                                        .AddContentTypeFieldLink(FieldModels.Contact)
                                                        .AddContentTypeFieldLink(FieldModels.Details);
                                                })
                                                .AddContentType(ContentTypeModels.CustomDocument, contentType =>
                                                {
                                                    contentType
                                                        .AddContentTypeFieldLink(FieldModels.Contact);
                                                })
                                                .AddContentType(ContentTypeModels.CustomChildDocument, contentType =>
                                                {
                                                }); ;
                                        });

            WithStaticSharePointClientContext(context =>
           {
               ServiceFactory.DeployModel(SiteModelHost.FromClientContext(context), model);
           });

        }

        #endregion
    }
}
