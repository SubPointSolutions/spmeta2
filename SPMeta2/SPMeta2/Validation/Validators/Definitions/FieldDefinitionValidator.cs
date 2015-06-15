using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;
using SPMeta2.Validation.Services;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class FieldDefinitionValidator : DefinitionBaseValidator
    {
        public override void Validate(DefinitionBase definition, List<ValidationResult> result)
        {
            Validate<FieldDefinition>(definition, model => model
                .NotNullString(m => m.Title, result)
                .NotEmptyString(m => m.Title, result)
                .NoSpacesBeforeOrAfter(m => m.Title, result)

                .NotNullString(m => m.Description, result)
                .NoSpacesBeforeOrAfter(m => m.Description, result)

                .NotNullString(m => m.InternalName, result)
                .NotEmptyString(m => m.InternalName, result)
                .NoMoreThan(m => m.InternalName, 32, result)
                .NoSpacesBeforeOrAfter(m => m.InternalName, result)

                .NotNullString(m => m.Group, result)
                .NoSpacesBeforeOrAfter(m => m.Group, result)

                .NotNullString(m => m.FieldType, result)
                .NotEmptyString(m => m.FieldType, result)
                .NoSpacesBeforeOrAfter(m => m.FieldType, result)

                .NotDefaultGuid(m => m.Id, result));
        }
    }
}
