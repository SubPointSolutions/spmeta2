using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Services.Impl;
using SPMeta2.Syntax.Default;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Services.Impl.Validation;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class DefaultNotAbsoluteUrlPropertiesValidationServiceTests : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public DefaultNotAbsoluteUrlPropertiesValidationServiceTests()
        {
            Service = new DefaultNotAbsoluteUrlPropertiesValidationService();
        }

        #endregion

        #region properties

        public DefaultNotAbsoluteUrlPropertiesValidationService Service { get; set; }


        #endregion

        #region versions

        [TestMethod]
        [TestCategory("Regression.Services.DefaultNotAbsoluteUrlPropertiesValidationService")]
        public void ShouldFail_On_Http()
        {
            var isValid = false;

            try
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddRandomUserCustomAction(userAction =>
                    {
                        var def = userAction.Value as UserCustomActionDefinition;

                        def.ScriptSrc = Rnd.HttpUrl();
                    });
                });

                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = IsCorrectValidationException(e);
            }

            Assert.IsTrue(isValid);
        }


        [TestMethod]
        [TestCategory("Regression.Services.DefaultNotAbsoluteUrlPropertiesValidationService")]
        public void ShouldFail_On_Https()
        {
            var isValid = false;

            try
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddRandomUserCustomAction(userAction =>
                    {
                        var def = userAction.Value as UserCustomActionDefinition;

                        def.ScriptSrc = Rnd.HttpsUrl();
                    });
                });

                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = IsCorrectValidationException(e);
            }

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultNotAbsoluteUrlPropertiesValidationService")]
        public void ShouldFail_On_SlashSlashUrl()
        {
            var isValid = false;

            try
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddRandomUserCustomAction(userAction =>
                    {
                        var def = userAction.Value as UserCustomActionDefinition;

                        def.ScriptSrc = "//ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js";
                    });
                });

                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = IsCorrectValidationException(e);
            }

            Assert.IsTrue(isValid);
        }



        #endregion

        #region passes


        [TestMethod]
        [TestCategory("Regression.Services.DefaultNotAbsoluteUrlPropertiesValidationService")]
        public void ShouldPass_On_TokenUrl()
        {
            var isValid = false;

            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomUserCustomAction(userAction =>
                {
                    var def = userAction.Value as UserCustomActionDefinition;

                    def.ScriptSrc = string.Format("~sitecollection/{0}.js", Rnd.String());
                });
            });

            Service.DeployModel(null, model);

            isValid = true;

            Assert.IsTrue(isValid);
        }

        #endregion
    }
}
