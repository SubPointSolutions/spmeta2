using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Collections
{
    public class WebpartPageCollectionValidator : CollectionValidatorBase
    {
        public override void Validate(IEnumerable<DefinitionBase> models, List<ValidationResult> result)
        {
            Validate<WebPartPageDefinition>(models, model =>
            {
                CheckIfUnique(model, m => m.FileName, result);
            });
        }
    }
}
