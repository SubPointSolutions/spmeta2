using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class SecurityRoleDefinitionValidator : ValidatorBase<SecurityRoleDefinition>
    {
        public override void Validate(SecurityRoleDefinition model, List<ValidationResult> result)
        {
            model
                .NotEmptyString(m => m.Name, result);
        }
    }
}
