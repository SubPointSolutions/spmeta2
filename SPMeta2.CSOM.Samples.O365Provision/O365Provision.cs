using System;
using System.Security;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using Microsoft.SharePoint.Client.Publishing.Navigation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Samples.O365Provision.Consts;
using SPMeta2.CSOM.Samples.O365Provision.Models;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.CSOM.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Samples.O365Provision
{
    //[TestClass]
    public class O365Provision : CSOMTestBase
    {
        #region constructors

        public O365Provision()
        {
            SiteUrl = Environment.GetEnvironmentVariable(EnvConsts.O365_SiteUrl); ;
            UserName = Environment.GetEnvironmentVariable(EnvConsts.O365_UserName);
            UserPassword = Environment.GetEnvironmentVariable(EnvConsts.O365_Password);
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("O365")]
        public void CanDeployFields()
        {
            WithO365Context(context =>
            {
                var metadataModel = SPMeta2Model
                    .NewSiteModel(site =>
                    {
                        site
                            .WithFields(fields =>
                            {
                                fields
                                    .AddField(FieldModels.ClientFeedback)
                                    .AddField(FieldModels.ClientRating)
                                    .AddField(FieldModels.Contact)
                                    .AddField(FieldModels.Details);
                            });
                    });

                var provisionService = new CSOMProvisionService();
                provisionService.DeployModel(SiteModelHost.FromClientContext(context), metadataModel);
            });
        }

        #endregion

    }
}
