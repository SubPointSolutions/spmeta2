using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Collections
{
    public abstract class CollectionValidatorBase : ValidatorBase<IEnumerable<DefinitionBase>>
    {
        #region methods

        protected void Validate<TModel>(IEnumerable<DefinitionBase> definition, Action<IEnumerable<TModel>> model)
            where TModel : DefinitionBase
        {
            var list = new List<TModel>();

            foreach (var l in definition)
                if (l is TModel)
                    list.Add(l as TModel);

            model(list);
        }

        protected void CheckIfUnique<TModel, TProperty>(
            IEnumerable<TModel> model,
            Expression<Func<TModel, TProperty>> propertyLambda,
            List<ValidationResult> result)
        {
            var uniqueValues = new Dictionary<TProperty, List<TModel>>();
            var propName = string.Empty;

            foreach (var m in model)
            {
                var propValue = ReflectionUtils.GetExpressionValue(m, propertyLambda);

                var key = (TProperty)propValue.Value;
                propName = propValue.Name;

                // null stuff is handled by otehr handlers
                if (key == null)
                    continue;

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
                    var message = string.Format("Found duplicated property: [{0}] with value: [{2}] for the type [{1}] and instancies: ",
                        propName,
                        typeof(TModel).Name,
                        key);

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
