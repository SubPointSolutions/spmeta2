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

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class DefaultContentTypeIdPropertyValidationServiceTests : SPMeta2RegresionScenarioTestBase
    {
        #region constructors

        public DefaultContentTypeIdPropertyValidationServiceTests()
        {
            Service = new DefaultContentTypeIdPropertyValidationService();
        }

        #endregion

        #region properties

        public DefaultContentTypeIdPropertyValidationService Service { get; set; }

        #endregion

        #region caml

        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeIdPropertyValidationService")]
        public void ShouldPass_On_Valid_ContentTypeId()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomContentType();
                site.AddRandomContentType();
            });

            Service.DeployModel(null, model);
        }


        [TestMethod]
        [TestCategory("Regression.Services.DefaultContentTypeIdPropertyValidationService")]
        public void ShouldFail_On_Invalid_ContentTypeId()
        {
            var isValid = false;

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomContentType(ct =>
                {
                    (ct.Value as ContentTypeDefinition).ParentContentTypeId = Guid.NewGuid().ToString();
                });
            });
            try
            {
                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = e is SPMeta2ModelDeploymentException
                          && e.InnerException is SPMeta2ModelValidationException;
            }

            Assert.IsTrue(isValid);
        }


        #endregion
    }
}
