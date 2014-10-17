using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Regression.Utils;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;
using System.Text;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Regression.Assertion;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWelcomePageDefinitionValidator : WelcomePageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<WelcomePageDefinition>("model", value => value.RequireNotNull());

            var spObject = ExtractFolderFromModelHost(modelHost);
            var context = spObject.Context;

            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject);

        }

        #endregion
    }
}
