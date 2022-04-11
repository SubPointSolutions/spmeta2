using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Services;
using SPMeta2.Services.Impl;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;
using System.Collections.Generic;
using SPMeta2.Syntax.Default;

using SPMeta2.Extensions;
using System;
using SPMeta2.Models;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class DefaultFluentModelValidationServiceTests : SPMeta2DefinitionRegresionTestBase
    {
        #region properties

        #endregion

        #region base tests

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void Can_Get_FluentModelValidationServiceBase_Instance()
        {
            var service = ServiceContainer.Instance.GetService<FluentModelValidationServiceBase>();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void Can_Create_DefaultFluentModelValidationService()
        {
            var service = new DefaultFluentModelValidationService();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void Can_Use_FluentModelValidationServiceBase_DefaultModelExtension()
        {
            var model = SPMeta2Model.NewSiteModel(site =>
            {

            });

            model.Validate(context =>
            {
                context.IsValid = true;
            });
        }

        #endregion

        #region usage tests

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void DefaultFluentModelValidationService_Pass_On_Invalid_Model()
        {
            var model = GetDefaultModel();

            var result = model.Validate(context =>
            {
                context.IsValid = false;
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
        }


        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void DefaultFluentModelValidationService_Pass_On_Valid_Model()
        {
            var model = GetDefaultModel();

            var result = model.Validate(context =>
            {
                context.IsValid = true;
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void DefaultFluentModelValidationService_Throw_On_Null_Action()
        {
            var model = GetDefaultModel();
            var result = model.Validate(null);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void DefaultFluentModelValidationService_Pass_On_Has_Hist()
        {
            var model = GetDefaultModel();

            var hasHit = false;

            var result = model.Validate(context =>
            {
                hasHit = true;
            });

            Assert.AreEqual(true, hasHit);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void DefaultFluentModelValidationService_Pass_On_ValidContext()
        {
            var model = GetDefaultModel();

            var hasHit = false;

            var result = model.Validate(context =>
            {
                Assert.IsNotNull(context.ValidationContext.CurrentDefinition);
                Assert.IsNotNull(context.ValidationContext.CurrentModelNode);

                hasHit = true;
            });

            Assert.AreEqual(true, hasHit);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void DefaultFluentModelValidationService_Pass_On_Invalid_Fields()
        {
            var model = GetFieldsModel();

            var result = model.Validate(context =>
            {
                if (context.ValidationContext.CurrentDefinition is FieldDefinition)
                {
                    var def = context.ValidationContext.CurrentDefinition as FieldDefinition;

                    if (string.IsNullOrEmpty(def.InternalName))
                    {
                        context.IsValid = false;
                    }
                    else
                    {
                        context.IsValid = true;
                    }
                }
                else
                {
                    context.IsValid = false;
                }
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void DefaultFluentModelValidationService_Pass_On_Valid_Fields()
        {
            var model = GetFieldsModel();

            foreach (var fieldMOdel in model.ChildModels)
            {
                (fieldMOdel.Value as FieldDefinition).InternalName = "t";
            }

            var result = model.Validate(context =>
            {
                if (context.ValidationContext.CurrentDefinition is FieldDefinition)
                {
                    var def = context.ValidationContext.CurrentDefinition as FieldDefinition;

                    if (string.IsNullOrEmpty(def.InternalName))
                    {
                        context.IsValid = false;
                    }
                    else
                    {
                        context.IsValid = true;
                    }
                }
                else
                {
                    context.IsValid = true;
                }
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsValid);
        }

        [TestMethod]
        [TestCategory("Regression.Services.DefaultFluentModelValidationService")]
        [TestCategory("CI.Core")]
        public void DefaultFluentModelValidationService_Pass_On_Fields_Only()
        {

            var model = GetFieldsModel();

            foreach (var fieldMOdel in model.ChildModels)
            {
                (fieldMOdel.Value as FieldDefinition).InternalName = "t";
            }

            var result = model.Validate<FieldDefinition>(context =>
            {
                context.IsValid = true;
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsValid);
        }

        #endregion

        #region utils

        private ModelNode GetDefaultModel()
        {
            return SPMeta2Model.NewSiteModel(site =>
            {

            }); ;
        }


        private ModelNode GetFieldsModel()
        {
            return SPMeta2Model.NewSiteModel(site =>
            {
                site.AddField(new FieldDefinition());
                site.AddField(new FieldDefinition());
                site.AddField(new FieldDefinition());
            }); ;
        }

        #endregion
    }
}
