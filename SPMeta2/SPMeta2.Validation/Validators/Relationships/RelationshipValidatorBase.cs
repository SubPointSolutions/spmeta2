using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Relationships
{
    public abstract class RelationshipValidatorBase : ValidatorBase<ModelNode>
    {
        #region methods

        protected virtual void ValidateAllowedTypes<TModel>(ModelNode model,
           IEnumerable<Type> types,
           List<ValidationResult> result)
           where TModel : DefinitionBase
        {
            if (!(model.Value is TModel))
                return;

            var allowedTypes = types.ToList();
            var childTypes = model.ChildModels
                .Select(m => m.Value.GetType())
                .GroupBy(t => t)
                .Select(t => t.Key)
                .ToList();


            foreach (var type in childTypes)
            {
                if (!allowedTypes.Contains(type))
                {
                    result.Add(new ValidationResult
                    {
                        IsValid = false,
                        Message = string.Format("Model definition of type [{0}] cannot contain child of type [{1}].",
                            model.Value.GetType(),
                            type)
                    });
                }
            }
        }

        #endregion
    }
}
