using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class FieldDefinitionValidator : ValidatorBase<FieldDefinition>
    {
        public override void Validate(FieldDefinition model, List<ValidationResult> result)
        {
            model
                .NotEmptyString(m => m.Title, result)
                .NotEmptyString(m => m.Description, result)

                .NotEmptyString(m => m.InternalName, result)
                .NoMoreThan(m => m.InternalName, 32, result)

                .NotEmptyString(m => m.Group, result)
                .NotEmptyString(m => m.FieldType, result)
                .NotDefaultGuid(m => m.Id, result);
        }
    }
}
