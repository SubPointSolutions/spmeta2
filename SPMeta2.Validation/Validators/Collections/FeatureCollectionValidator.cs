using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Collections
{
    public class FeatureDefinitionCollectionValidator : CollectionValidatorBase<FeatureDefinition>
    {
        public override void Validate(List<FeatureDefinition> model, List<ValidationResult> result)
        {
            CheckIfUnique(model, m => m.Id, result);
        }
    }
}
