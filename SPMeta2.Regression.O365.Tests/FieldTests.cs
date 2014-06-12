using System;
using System.Security;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.Regression.Model;
using SPMeta2.Regression.O365.Tests.Common;

namespace SPMeta2.Regression.O365.Tests
{
    [TestClass]
    public class FieldTests : CSOMTestBase
    {
        #region tests

        [TestMethod]
        public void CanDeployFieldsToSite()
        {
            WithO365Context(context =>
            {
                var model = new RegModel().GetSiteFields();

                var provisionService = new CSOMProvisionService();
                provisionService.DeployModel(SiteModelHost.FromClientContext(context), model);
            });
        }

        [TestMethod]
        public void CanDeployFieldsToList()
        {

        }

        #endregion
    }
}
