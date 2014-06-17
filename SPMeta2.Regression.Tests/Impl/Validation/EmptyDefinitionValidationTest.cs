using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Services;

namespace SPMeta2.Regression.Tests.Impl.Validation
{
    [TestClass]
    public class EmptyDefinitionValidationTest
    {
        #region definitions

        [TestMethod]
        [TestCategory("Regression.Validation.Definition")]
        public void CanValidateRequiredProperties_FieldDefinition()
        {
            CanValidateRequiredPropertiesOnSiteModel<FieldDefinition>((def, result) =>
            {
                AssertProperty(def, result, ValidationResultType.NotNullString, d => d.Title);
                AssertProperty(def, result, ValidationResultType.NotEmptyString, d => d.Title);

                AssertProperty(def, result, ValidationResultType.NotNullString, d => d.InternalName);
                AssertProperty(def, result, ValidationResultType.NotEmptyString, d => d.InternalName);

                //AssertProperty(def, result, ValidationResultType.NotNullString, d => d.Description);
                // AssertProperty(def, result, ValidationResultType.NotNullString, d => d.Group);

                AssertProperty(def, result, ValidationResultType.NotNullString, d => d.FieldType);
                AssertProperty(def, result, ValidationResultType.NotEmptyString, d => d.FieldType);

                AssertProperty(def, result, ValidationResultType.NotDefaultGuid, d => d.Id);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Validation.Definition")]
        public void CanValidateProperties_FieldDefinition()
        {
            CanValidatePropertiesOnSiteModel(new FieldDefinition
            {
                Title = "Test Field",
                InternalName = "Test Internal Name",
                Description = string.Empty,
                Id = Guid.NewGuid(),
                FieldType = "Text",
                Group = string.Empty
            }, result => Assert.AreEqual(0, result.Count));
        }

        [TestMethod]
        [TestCategory("Regression.Validation.Definition")]
        public void CanValidateRequiredProperties_ContentTypeDefinition()
        {
            CanValidateRequiredPropertiesOnSiteModel<ContentTypeDefinition>((def, result) =>
            {
                AssertProperty(def, result, ValidationResultType.NotNullString, d => d.Name);
                AssertProperty(def, result, ValidationResultType.NotEmptyString, d => d.Name);

                AssertProperty(def, result, ValidationResultType.NotDefaultGuid, d => d.Id);

                AssertProperty(def, result, ValidationResultType.NotNullString, d => d.ParentContentTypeId);
                AssertProperty(def, result, ValidationResultType.NotEmptyString, d => d.ParentContentTypeId);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Validation.Definition")]
        public void CanValidateRequiredProperties_ListDefinition()
        {
            CanValidateRequiredPropertiesOnSiteModel<ListDefinition>((def, result) =>
            {
                AssertProperty(def, result, ValidationResultType.NotNullString, d => d.Title);
                AssertProperty(def, result, ValidationResultType.NotEmptyString, d => d.Title);

                AssertProperty(def, result, ValidationResultType.NotEmptyString, d => d.Description);

                AssertProperty(def, result, ValidationResultType.NotNullString, d => d.Url);
                AssertProperty(def, result, ValidationResultType.NotEmptyString, d => d.Url);

                AssertProperty(def, result, ValidationResultType.NotEqual, d => d.TemplateType);

                AssertProperty(def, result, ValidationResultType.NotNullString, d => d.TemplateName);
                AssertProperty(def, result, ValidationResultType.NotEmptyString, d => d.TemplateName);
            });
        }

        #endregion

        #region static

        private static void CanValidatePropertiesOnSiteModel<TModel>(
            TModel def,
            Action<List<ValidationResult>> action)
            where TModel : DefinitionBase
        {
            var validationService = new ModelValidationService();

            var model = SPMeta2Model.NewSiteModel();
            model.ChildModels.Add(new ModelNode { Value = def });

            validationService.DeployModel(null, model);

            var result = validationService.Result;

            action(result);
        }

        private static void CanValidateRequiredPropertiesOnSiteModel<TModel>(Action<TModel, List<ValidationResult>> action)
           where TModel : DefinitionBase, new()
        {
            var validationService = new ModelValidationService();
            var def = new TModel();

            var model = SPMeta2Model.NewSiteModel();
            model.ChildModels.Add(new ModelNode { Value = def });

            validationService.DeployModel(null, model);

            var result = validationService.Result;

            action(def, result);
        }

        private static void AssertProperty<TSource, TProperty>(TSource source,
            List<ValidationResult> result,
            ValidationResultType resultType,
            Expression<Func<TSource, TProperty>> exp)
        {
            var prop = ReflectionUtils.GetExpressionValue(source, exp);

            Assert.IsTrue(
              result.Count(r => r.PropertyName == prop.Name &&
              !r.IsValid &&
              r.ResultType == resultType) > 0);
        }


        #endregion
    }
}
