using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Collections
{
    public class ListCollectionValidator : CollectionValidatorBase
    {
        public override void Validate(IEnumerable<DefinitionBase> models, List<ValidationResult> result)
        {
            Validate<ListDefinition>(models, model =>
            {
                CheckIfUnique(model, m => m.Title, result);
#pragma warning disable 618
                CheckIfUnique(model, m => m.Url, result);
#pragma warning restore 618
            });
        }
    }
}
