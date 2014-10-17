using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Services;

namespace SPMeta2.Regression.Tests.Impl.Validation
{
    //[TestClass]
    public class EmptyDefinitionValidationTest
    {
        #region definitions

        [TestMethod]
        [TestCategory("Regression.Validation.Definition")]
        public void CanValidateRequiredProperties_FieldDefinition()
        {
            CanValidateRequiredPropertiesOnSiteModel<FieldDefinition>(context => context
                .AssertProperty(ValidationResultType.NotNullString, d => d.Title)
                .AssertProperty(ValidationResultType.NotEmptyString, d => d.Title)

                .AssertProperty(ValidationResultType.NotNullString, d => d.InternalName)
                .AssertProperty(ValidationResultType.NotEmptyString, d => d.InternalName)

                .AssertProperty(ValidationResultType.NotNullString, d => d.Group)

                .AssertProperty(ValidationResultType.NotNullString, d => d.FieldType)
                .AssertProperty(ValidationResultType.NotEmptyString, d => d.FieldType)

                .AssertProperty(ValidationResultType.NotDefaultGuid, d => d.Id));
        }

        [TestMethod]
        [TestCategory("Regression.Validation.Definition")]
        public void CanValidateRequiredProperties_ContentTypeDefinition()
        {
            CanValidateRequiredPropertiesOnSiteModel<ContentTypeDefinition>(context => context
                .AssertProperty(ValidationResultType.NotNullString, d => d.Name)
                .AssertProperty(ValidationResultType.NotEmptyString, d => d.Name)

                .AssertProperty(ValidationResultType.NotDefaultGuid, d => d.Id)

                .AssertProperty(ValidationResultType.NotNullString, d => d.ParentContentTypeId)
                .AssertProperty(ValidationResultType.NotEmptyString, d => d.ParentContentTypeId));
        }

        [TestMethod]
        [TestCategory("Regression.Validation.Definition")]
        public void CanValidateRequiredProperties_ListDefinition()
        {
            CanValidateRequiredPropertiesOnSiteModel<ListDefinition>(context => context
                .AssertProperty(ValidationResultType.NotNullString, d => d.Title)
                .AssertProperty(ValidationResultType.NotEmptyString, d => d.Title)

                .AssertProperty(ValidationResultType.NotEmptyString, d => d.Description)

                .AssertProperty(ValidationResultType.NotNullString, d => d.Url)
                .AssertProperty(ValidationResultType.NotEmptyString, d => d.Url)

                .AssertProperty(ValidationResultType.NotEqual, d => d.TemplateType)

                .AssertProperty(ValidationResultType.NotNullString, d => d.TemplateName)
                .AssertProperty(ValidationResultType.NotEmptyString, d => d.TemplateName));
        }

        #endregion

        #region static

        private static void CanValidatePropertiesOnSiteModel<TModel>(TModel def, Action<List<ValidationResult>> action)
            where TModel : DefinitionBase
        {
            var validationService = new ModelValidationService();

            var model = SPMeta2Model.NewSiteModel();
            model.ChildModels.Add(new ModelNode { Value = def });

            validationService.DeployModel(null, model);

            var result = validationService.Result;

            action(result);
        }

        private static void CanValidateRequiredPropertiesOnSiteModel<TModel>(Action<ValidationPair<TModel>> action)
           where TModel : DefinitionBase, new()
        {
            var validationService = new ModelValidationService();
            var def = new TModel();

            var model = SPMeta2Model.NewSiteModel();
            model.ChildModels.Add(new ModelNode { Value = def });

            validationService.DeployModel(null, model);

            action(new ValidationPair<TModel>
            {
                Model = def,
                ValidationResult = validationService.Result
            });
        }

        #endregion
    }

    public class ValidationPair<TModel>
           where TModel : DefinitionBase
    {
        public TModel Model { get; set; }
        public List<ValidationResult> ValidationResult { get; set; }
    }

    public static class Helper
    {
        public static ValidationPair<TSource> AssertProperty<TSource, TProperty>(
           this ValidationPair<TSource> source,
           ValidationResultType resultType,
           Expression<Func<TSource, TProperty>> exp)
            where TSource : DefinitionBase
        {
            var result = source.ValidationResult;
            var prop = ReflectionUtils.GetExpressionValue(source.Model, exp);

            Assert.IsTrue(
              result.Count(r => r.PropertyName == prop.Name &&
              !r.IsValid &&
              r.ResultType == resultType) > 0);

            return source;
        }
    }
}
