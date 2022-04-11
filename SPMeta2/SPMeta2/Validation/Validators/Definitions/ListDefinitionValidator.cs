using System.Collections.Generic;

using SPMeta2.Definitions;
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

#pragma warning disable 618
                    .NotNullString(m => m.Url, result)
#pragma warning restore 618
#pragma warning disable 618
                    .NotEmptyString(m => m.Url, result)
#pragma warning restore 618
#pragma warning disable 618
                    .NoSpacesBeforeOrAfter(m => m.Url, result);
#pragma warning restore 618

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
