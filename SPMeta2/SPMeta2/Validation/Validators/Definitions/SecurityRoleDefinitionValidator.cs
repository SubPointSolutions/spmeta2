using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class SecurityRoleDefinitionValidator : DefinitionBaseValidator
    {
        public override void Validate(DefinitionBase definition, List<ValidationResult> result)
        {
            Validate<SecurityRoleDefinition>(definition, model => model
                .NotEmptyString(m => m.Name, result)
                .NoSpacesBeforeOrAfter(m => m.Name, result));
        }
    }
}
