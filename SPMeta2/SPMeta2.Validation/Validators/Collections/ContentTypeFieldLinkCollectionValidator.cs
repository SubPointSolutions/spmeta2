using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Collections
{
    public class ContentTypeFieldLinkCollectionValidator : CollectionValidatorBase
    {
        public override void Validate(IEnumerable<DefinitionBase> models, List<ValidationResult> result)
        {
            Validate<ContentTypeFieldLinkDefinition>(models, model =>
            {
                CheckIfUnique(model, m => m.FieldId, result);
            });
        }
    }
}
