using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Services.Impl;
using SPMeta2.Syntax.Default;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Services.Impl.Validation;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class DefaultFieldInternalNamePropertyValidationServiceTests : SPMeta2DefinitionRegresionTestBase
    {
        #region constructors

        public DefaultFieldInternalNamePropertyValidationServiceTests()
        {
            Service = new DefaultFieldInternalNamePropertyValidationService();
        }

        #endregion

        #region properties

        public DefaultFieldInternalNamePropertyValidationService Service { get; set; }

        #endregion

        #region internal field name

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFieldInternalNamePropertyValidationServiceTests")]
        [TestCategory("CI.Core")]
        public void ShouldPass_On_InternalName_LessThan_32()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField();
            });

            Service.DeployModel(null, model);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFieldInternalNamePropertyValidationServiceTests")]
        [TestCategory("CI.Core")]
        public void ShouldPass_On_Empty_InternalName()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(new FieldDefinition
                {

                });
            });

            Service.DeployModel(null, model);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFieldInternalNamePropertyValidationServiceTests")]
        [TestCategory("CI.Core")]
        public void ShouldFail_On_InternalName_MoreThan_32()
        {
            var isValid = false;

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site.AddRandomField(ct =>
                {
                    (ct.Value as FieldDefinition).InternalName = Rnd.String(33);
                });
            });
            try
            {
                Service.DeployModel(null, model);
            }
            catch (Exception e)
            {
                isValid = IsCorrectValidationException(e);
            }

            Assert.IsTrue(isValid);
        }


        #endregion
    }
}
