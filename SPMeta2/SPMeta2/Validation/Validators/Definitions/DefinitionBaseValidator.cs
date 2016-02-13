using System;
using SPMeta2.Definitions;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Validators.Definitions
{
    public abstract class DefinitionBaseValidator : ValidatorBase<DefinitionBase>
    {
        protected void Validate<TModel>(DefinitionBase definition, Action<TModel> model)
            where TModel : DefinitionBase
        {
            if (definition is TModel)
                model(definition as TModel);
        }
    }
}
