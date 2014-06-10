using System;
using System.Security;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using Microsoft.SharePoint.Client.Publishing.Navigation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Samples.O365Provision.Models;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.CSOM.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Samples.O365Provision
{
    [TestClass]
    public class O365Provision : CSOMTestBase
    {
        #region constructors

        public O365Provision()
        {

        }

        #endregion

        [TestMethod]
        [TestCategory("O365")]
        public void CanGetConfigFile()
        {
            var configExist = System.IO.File.Exists("test.config");

            Assert.AreEqual(true, configExist);
        }

        [TestMethod]
        [TestCategory("O365")]
        public void CanRetractFields()
        {
            var model = SPMeta2Model
                                    .NewSiteModel(site =>
                                    {
                                        site.WithFields(fields =>
                                        {
                                            fields
                                                .AddField(FieldModels.ClientFeedback)
                                                .AddField(FieldModels.ClientRating)
                                                .AddField(FieldModels.Contact)
                                                .AddField(FieldModels.Details);
                                        });

                                    });

            WithO365Context(SiteUrl, context =>
            {
                new CSOMProvisionService().RetractModel(SiteModelHost.FromClientContext(context), model);
            });
        }


        [TestMethod]
        [TestCategory("O365")]
        public void CanRetractContentType()
        {
            var model = SPMeta2Model
                                    .NewSiteModel(site =>
                                    {
                                        site.WithContentTypes(contentTypes =>
                                        {
                                            contentTypes
                                                .AddContentType(ContentTypeModels.Client)
                                                .AddContentType(ContentTypeModels.ClientDocument);
                                        });
                                    });

            WithO365Context(SiteUrl, context =>
            {
                new CSOMProvisionService().RetractModel(SiteModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("O365")]
        public void CanRetractWeb()
        {
            var webDef = new WebDefinition
            {
                Title = "test web",
                Description = String.Empty,
                WebTemplate = "BDR#0",
                Url = "test-web",
            };

            WithO365Context(SiteUrl, context =>
            {
                var model = SPMeta2Model
                    .NewWebModel(webDef, web =>
                    {

                    });

                new CSOMProvisionService().RetractModel(WebModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("O365")]
        public void CanDeployListItem()
        {
            WithO365Context(SiteUrl, context =>
            {
                var model = SPMeta2Model
                    .NewWebModel(web =>
                    {
                        web
                            .AddList(new ListDefinition()
                            {
                                Url = "Tasks",
                                Title = "Tasks",
                                TemplateType = BuiltInListTemplateTypeId.TasksWithTimelineAndHierarchy
                            }, list =>
                            {
                                list
                                    .AddListItem(new ListItemDefinition
                                    {
                                        Title = "task 1"
                                    }, item =>
                                    {
                                        item
                                            .AddListItemFieldValue("Title", Environment.TickCount)
                                            .AddListItemFieldValue("Body", Environment.TickCount)
                                            .AddListItemFieldValue("DueDate", DateTime.Now);
                                    })
                                    .AddListItem(new ListItemDefinition
                                    {
                                        Title = "task 2"
                                    }, item =>
                                    {
                                        item
                                            .AddListItemFieldValue("Body", Guid.NewGuid().ToString())
                                            .AddListItemFieldValue("DueDate", DateTime.Now.AddMonths(1));
                                    });
                            });
                    });


                new CSOMProvisionService().DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("O365")]
        public void CanCreateWeb()
        {
            new CSOMProvisionService().DeployModel(null, null);

            WithO365Context(SiteUrl, context =>
            {
                var model = SPMeta2Model
                    .NewModel()
                    .AddWeb(new WebDefinition
                    {
                        Title = "test",
                        Url = "test",
                        WebTemplate = "STS#0"
                    });

                new CSOMProvisionService().DeployModel(WebModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        [TestCategory("O365")]
        public void CanConnectToO365()
        {
            WithO365Context(context =>
            {
                var metadataModel = SPMeta2Model
                    .NewModel()
                    .DummySite()
                    .WithFields(fields =>
                    {
                        fields
                            .AddField(FieldModels.ClientFeedback)
                            .AddField(FieldModels.ClientRating)
                            .AddField(FieldModels.Contact)
                            .AddField(FieldModels.Details);
                    })
                    .WithContentTypes(contentTypes =>
                    {
                        contentTypes
                            .AddContentType(ContentTypeModels.Client, clientContentType =>
                            {
                                clientContentType
                                    .AddContentTypeFieldLink(FieldModels.ClientFeedback)
                                    .AddContentTypeFieldLink(FieldModels.ClientRating);
                            })
                            .AddContentType(ContentTypeModels.ClientDocument, clientDocument =>
                            {
                                clientDocument
                                    .AddContentTypeFieldLink(FieldModels.ClientFeedback)
                                    .AddContentTypeFieldLink(FieldModels.ClientRating)
                                    .AddContentTypeFieldLink(FieldModels.Contact)
                                    .AddContentTypeFieldLink(FieldModels.Details);
                            });
                    });

                var provisionService = new CSOMProvisionService();
                provisionService.DeployModel(SiteModelHost.FromClientContext(context), metadataModel);
            });
        }
    }
}
