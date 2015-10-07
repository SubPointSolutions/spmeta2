using System.Collections.Generic;

namespace SPMeta2.Validation.Common
{
    public abstract class ValidatorBase<TTargetType>
    {
        public TTargetType TargetType { get; set; }

        public string Title { get; set; }
        public abstract void Validate(TTargetType model, List<ValidationResult> result);
    }
}
