using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class ContentTypeDefinitionValidator : ValidatorBase<ContentTypeDefinition>
    {
        public override void Validate(ContentTypeDefinition model, List<ValidationResult> result)
        {
            //model
            //    .NotEmptyString(m => m.Description, result)
            //    .NotEmptyString(m => m.Title, result);
        }
    }
}
