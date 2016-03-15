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
    public class DefaultVersionBasedPropertiesValidationServiceTests : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public DefaultVersionBasedPropertiesValidationServiceTests()
        {
            Service = new DefaultVersionBasedPropertiesValidationService();
        }

        #endregion

        #region properties

        public DefaultVersionBasedPropertiesValidationService Service { get; set; }


        #endregion

        #region versions

        [TestMethod]
        [TestCategory("Regression.Services.DefaultVersionBasedPropertiesValidationService.Version")]
        public void ShouldFail_On_Invalid_Version()
        {
            var isValid = false;

            try
            {
                var model = SPMeta2Model.NewWebModel(web =>
                {
                    web.AddRandomApp(app =>
                    {
                        var appDef = app.Value as AppDefinition;

                        appDef.Version = Rnd.String();
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
        [TestCategory("Regression.Services.DefaultVersionBasedPropertiesValidationService.Version")]
        public void ShouldPass_On_Valid_Version()
        {
            var model = SPMeta2Model.NewWebModel(web =>
            {
                web.AddRandomApp(app =>
                {
                    var appDef = app.Value as AppDefinition;

                    appDef.Version = Rnd.VersionString();
                });
            });

            Service.DeployModel(null, model);
        }


        #endregion
    }
}
