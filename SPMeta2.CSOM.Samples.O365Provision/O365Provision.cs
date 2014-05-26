using System;
using System.Security;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Samples.O365Provision.Models;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
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
            var siteUrl = "https://pademo3.sharepoint.com/sites/101-uat";
            var webUrl = siteUrl + "/whs";

            SiteUrl = siteUrl;
            WebUrl = webUrl;

            UserName = "admin@pademo3.onmicrosoft.com";
            UserPassword = "Profe$$ional124";
        }

        #endregion

        [TestMethod]
        public void CanT()
        {
            WithO365Context(SiteUrl,context =>
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
                provisionService.DeployModel(context.Site, metadataModel);
            });
        }
    }
}
