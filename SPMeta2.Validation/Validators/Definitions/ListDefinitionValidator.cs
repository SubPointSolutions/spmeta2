using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class ListDefinitionValidator : DefinitionBaseValidator
    {
        public override void Validate(DefinitionBase definition, List<ValidationResult> result)
        {
            Validate<ListDefinition>(definition, model => model
                 .NotEmptyString(m => m.Title, result)
                 .NoSpacesBeforeOrAfter(m => m.Title, result)

                 .NotEmptyString(m => m.Description, result)
                 .NoSpacesBeforeOrAfter(m => m.Description, result)

                 .NotEmptyString(m => m.Url, result)
                 .NoSpacesBeforeOrAfter(m => m.Url, result));
            ;
        }
    }
}
