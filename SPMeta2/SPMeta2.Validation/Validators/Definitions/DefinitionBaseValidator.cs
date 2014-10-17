using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
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
