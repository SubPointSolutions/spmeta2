using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class ListDefinitionValidator : DefinitionBaseValidator
    {
        public override void Validate(DefinitionBase definition, List<ValidationResult> result)
        {
            Validate<ListDefinition>(definition, model =>
            {
                model
                    .NotNullString(m => m.Title, result)
                    .NotEmptyString(m => m.Title, result)
                    .NoSpacesBeforeOrAfter(m => m.Title, result)

                    .NotNullString(m => m.Description, result)
                    .NotEmptyString(m => m.Description, result)
                    .NoSpacesBeforeOrAfter(m => m.Description, result)

                    .NotNullString(m => m.Url, result)
                    .NotEmptyString(m => m.Url, result)
                    .NoSpacesBeforeOrAfter(m => m.Url, result);

                if (model.TemplateType == 0)
                {
                    model
                        .NotNullString(m => m.TemplateName, result)
                        .NotEmptyString(m => m.TemplateName, result)
                        .NoSpacesBeforeOrAfter(m => m.TemplateName, result);
                }

                if (string.IsNullOrEmpty(model.TemplateName))
                {
                    model
                        .NotEqual(m => m.TemplateType, 0, result);
                }
            });

        }
    }
}
