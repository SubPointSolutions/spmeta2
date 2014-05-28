using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class SecurityGroupDefinitionValidator : ValidatorBase<SecurityGroupDefinition>
    {
        public override void Validate(SecurityGroupDefinition model, List<ValidationResult> result)
        {
            model
                .NotEmptyString(m => m.Name, result);
        }
    }
}
