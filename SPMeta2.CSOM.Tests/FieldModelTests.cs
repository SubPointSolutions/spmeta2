using System;
using Microsoft.SharePoint.BusinessData.MetadataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Standard.Behaviours;
using SPMeta2.CSOM.Tests.Base;
using SPMeta2.CSOM.Tests.Models;
using SPMeta2.Definitions;
using SPMeta2.CSOM.DefaultSyntax;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Tests
{
    //[TestClass]
    public class FieldModelTests : ClientOMSharePointTestBase
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
        public void CanDeployFieldsToList()
        {
            var model = new ModelNode { Value = new WebDefinition { RequireSelfProcessing = false } }
                .WithLists(lists =>
                {
                    lists
                        .AddList(ListModels.TestList, list =>
                        {
                            list
                                 .AddField(FieldModels.Contact)
                                            .AddField(FieldModels.Details)
                                            .AddField(FieldModels.Details1)
                                            .AddField(FieldModels.Details2);

                        });
                });

            WithStaticSharePointClientContext(context =>
           {
               ServiceFactory.DeployModel(WebModelHost.FromClientContext(context), model);
           });
        }

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeployFieldModel()
        {
            var model = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                                    .WithFields(fields =>
                                    {
                                        fields
                                            .AddField(FieldModels.Contact)
                                            .AddField(FieldModels.Details)
                                            .AddField(FieldModels.Details1)
                                            .AddField(FieldModels.Details2);
                                    });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(SiteModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("CSOM")]
        public void CanDeployManagedMetadataFieldModel()
        {
            var model = new ModelNode { Value = new SiteDefinition { RequireSelfProcessing = false } }
                .WithFields(fields =>
                {
                    fields
                        .AddField(FieldModels.TaxCategory, field =>
                        {
                            field.OnCreated((FieldDefinition FieldDefinition, Microsoft.SharePoint.Client.Field f) =>
                            {
                                f.MakeConnectionToTermSet(
                                    new Guid("06a77b64-2ae0-4499-adaa-c60ceaac6a0d"),
                                    new Guid("8ed8c9ea-7052-4c1d-a4d7-b9c10bffea6f"));

                            });
                        })
                        .AddField(FieldModels.TaxCategoryMulti, field =>
                        {
                            field.OnCreated((FieldDefinition FieldDefinition, Microsoft.SharePoint.Client.Field f) =>
                            {
                                f.MakeConnectionToTermSet(
                                    new Guid("06a77b64-2ae0-4499-adaa-c60ceaac6a0d"),
                                    new Guid("8ed8c9ea-7052-4c1d-a4d7-b9c10bffea6f"));

                            });
                        });
                });

            WithStaticSharePointClientContext(context =>
            {
                ServiceFactory.DeployModel(SiteModelHost.FromClientContext(context), model);
            });
        }

        #endregion
    }
}
