using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Regression;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.DefinitionGenerators.Fields;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Services;
using SPMeta2.Exceptions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Extended;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class PublishingPageDefinitionTests : SPMeta2RegresionScenarioTestBase
    {
        #region common

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region properties

        [TestMethod]
        [TestCategory("Regression.Definitions.PublishingPageDefinition")]
        public void PublishingPageDefinition_ShouldCheck_PageLayoutFileName()
        {
            // testing only ncorrect case, positives are tested with the deployment
            var pageDef = ModelGeneratorService.GetRandomDefinition<PublishingPageDefinition>(def =>
            {
                def.PageLayoutFileName = Rnd.String();
            });

            var siteModel = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddFeature(BuiltInSiteFeatures.SharePointServerPublishingInfrastructure.Inherit(def =>
                {
                    def.Enable = true;
                    def.ForceActivate = true;
                }));
            });

            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddFeature(BuiltInWebFeatures.SharePointServerPublishing.Inherit(def =>
                {
                    def.Enable = true;
                    def.ForceActivate = true;
                }));

                web.AddList(BuiltInListDefinitions.Pages.Inherit(), list =>
                {
                    list.AddPublishingPage(pageDef);
                });
            });

            var hasException = false;

            try
            {
                TestModel(siteModel, webModel);
            }
            catch (Exception ex)
            {
                hasException = true;
                hasException = IsValidException(ex);
            }

            Assert.IsTrue(hasException);
        }

        protected bool IsValidException(Exception ex)
        {
            return ex is SPMeta2ModelDeploymentException
               && (ex.InnerException as AggregateException).InnerException is SPMeta2ModelValidationException;
        }

        #endregion
    }
}
