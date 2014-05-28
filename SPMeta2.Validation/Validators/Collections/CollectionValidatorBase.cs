using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Collections
{
    public abstract class CollectionValidatorBase<TModel> : ValidatorBase<List<TModel>>
        where TModel : DefinitionBase
    {
        #region methods

        protected void CheckIfUnique<TModel, TProperty>(
            List<TModel> model,
            Expression<Func<TModel, TProperty>> propertyLambda,
            List<ValidationResult> result)
        {
            var uniqueValues = new Dictionary<TProperty, List<TModel>>();
            var propName = string.Empty;

            foreach (var m in model)
            {
                var propValue = ValidationExtensions.GetPropertyValue(m, propertyLambda);

                var key = (TProperty)propValue.Value;
                propName = propValue.Name;

                if (!uniqueValues.ContainsKey(key))
                {
                    uniqueValues.Add(key, new List<TModel>());
                    uniqueValues[key].Add(m);
                }
                else
                {
                    uniqueValues[key].Add(m);
                }
            }

            foreach (var key in uniqueValues.Keys)
            {
                var models = uniqueValues[key];
                var modelsCount = models.Count;

                if (modelsCount > 1)
                {
                    var message = string.Format("Found duplicated property: [{0}] for the type [{1}] and instancies: ",
                        propName, typeof(TModel).Name);

                    foreach (var m in models)
                        message += string.Format(" [{0}]", m.ToString());

                    result.Add(new ValidationResult
                    {
                        IsValid = false,
                        Message = message
                    });
                }
            }
        }

        #endregion
    }
}
