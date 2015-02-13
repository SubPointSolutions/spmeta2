using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Relationships
{
    public class FieldDefinitionModelNodeValidator : RelationshipValidatorBase
    {
        public override void Validate(ModelNode model, List<ValidationResult> result)
        {
            ValidateAllowedTypes<FieldDefinition>(model, Enumerable.Empty<Type>(), result);
        }
    }
}
