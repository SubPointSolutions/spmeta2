using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class ListDefinitionValidator : ValidatorBase<ListDefinition>
    {
        public override void Validate(ListDefinition model, List<ValidationResult> result)
        {
            model
                .NotEmptyString(m => m.Title, result)
                .NotEmptyString(m => m.Description, result)
                .NotEmptyString(m => m.Url, result);
        }
    }
}
