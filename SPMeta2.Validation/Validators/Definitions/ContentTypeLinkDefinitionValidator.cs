using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;
using SPMeta2.Validation.Extensions;

namespace SPMeta2.Validation.Validators.Definitions
{
    public class ContentTypeLinkDefinitionValidator : DefinitionBaseValidator
    {
        public override void Validate(DefinitionBase definition, List<ValidationResult> result)
        {
            Validate<ContentTypeLinkDefinition>(definition, model => model
                .NotEmptyString(m => m.ContentTypeName, result)
                .NoSpacesBeforeOrAfter(m => m.ContentTypeName, result)

                .NotEmptyString(m => m.ContentTypeId, result)
                .NoSpacesBeforeOrAfter(m => m.ContentTypeId, result));
        }
    }
}
