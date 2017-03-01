using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using System.Collections.Generic;

namespace SPMeta2.Regression.Tests.Impl.Validation
{
    [TestClass]
    public class SecurityGroupValidationTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region group link options

        [TestMethod]
        [TestCategory("Regression.Validation.PublishingPageLayout")]
        public void Validation_SecurityGroup_ShouldAllow_EmptyName_If_BuiltIn()
        {
            var builtInGroups = new List<SecurityGroupDefinition>()
            {
                BuiltInSecurityGroupDefinitions.AssociatedMemberGroup,
                BuiltInSecurityGroupDefinitions.AssociatedOwnerGroup,
                BuiltInSecurityGroupDefinitions.AssociatedVisitorsGroup
            };

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddSecurityGroups(builtInGroups);
            });

            TestModels(new ModelNode[] { siteModel });
        }

        #endregion
    }
}
