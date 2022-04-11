using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Services.Impl;
using SPMeta2.Syntax.Default;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Services.Impl.Validation;
using SPMeta2.Regression.Utils;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class DefaultRequiredPropertyValidationServiceTests : SPMeta2DefinitionRegresionTestBase
    {
        #region constructors

        public DefaultRequiredPropertyValidationServiceTests()
        {
            Service = new DefaultRequiredPropertiesValidationService();
        }

        #endregion

        #region properties

        public DefaultRequiredPropertiesValidationService Service { get; set; }

        #endregion

        #region caml

        [TestMethod]
        [TestCategory("Regression.Services.DefaultRequiredPropertiesValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldPass_On_Valid_Field()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField();
            });

            Service.DeployModel(null, model);
        }


        [TestMethod]
        [TestCategory("Regression.Services.DefaultRequiredPropertiesValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_Invalid_Field_By_InternalName()
        {
            var isValid = false;

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField(field =>
                {
                    (field.Value as FieldDefinition).InternalName = string.Empty;
                });
            });
            try
            {
                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = e is SPMeta2ModelDeploymentException
                         && e.InnerException is SPMeta2AggregateException
                         && (e.InnerException as AggregateException)
                               .InnerExceptions.All(ie => ie is SPMeta2ModelValidationException);
            }

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultRequiredPropertiesValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_Invalid_Field_Twice()
        {
            var isValid = false;
            var innerExceptionCount = 0;

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField(field =>
                {
                    (field.Value as FieldDefinition).InternalName = string.Empty;
                    (field.Value as FieldDefinition).Id = default(Guid);
                });
            });
            try
            {
                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = e is SPMeta2ModelDeploymentException
                          && e.InnerException is SPMeta2AggregateException
                          && (e.InnerException as AggregateException)
                              .InnerExceptions.All(ie => ie is SPMeta2ModelValidationException);

                innerExceptionCount = (e.InnerException as AggregateException)
                    .InnerExceptions.Count;

                foreach (var ex in (e.InnerException as AggregateException).InnerExceptions
                                .OfType<SPMeta2ModelValidationException>())
                {
                    RegressionUtils.WriteLine("Ex: " + ex.Message + " N:" + ex.Definition);
                }
            }

            Assert.IsTrue(isValid);
            Assert.AreEqual(2, innerExceptionCount);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultRequiredPropertiesValidationService")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_Invalid_Field_By_Id()
        {
            var isValid = false;

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField(field =>
                {
                    (field.Value as FieldDefinition).Id = default(Guid);
                });
            });
            try
            {
                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = e is SPMeta2ModelDeploymentException
                          && e.InnerException is SPMeta2AggregateException
                          && (e.InnerException as AggregateException)
                                .InnerExceptions.All(ie => ie is SPMeta2ModelValidationException);
            }

            Assert.IsTrue(isValid);
        }


        #endregion
    }
}
