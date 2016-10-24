using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Common;
using SPMeta2.Containers.Services.Base;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Utils;

namespace SubPointSolutions.Docs.Views.SPMeta2.extensibility
{

    public class RegressionTestingBase : ProvisionTestBase
    {
        protected void TestRandomDefinition<WebDefinition>() { }
    }

    public class Regressiontesting : RegressionTestingBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Regression.Rnd.Web")]
        public void CanDeployRandom_WebDefinition()
        {
            TestRandomDefinition<WebDefinition>();
        }

        #endregion
    }

    public class WebDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WebDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.Description = Rnd.String();


                def.Url = Rnd.String(16);

                def.WebTemplate = BuiltInWebTemplates.Collaboration.TeamSite;
            });
        }
    }

}