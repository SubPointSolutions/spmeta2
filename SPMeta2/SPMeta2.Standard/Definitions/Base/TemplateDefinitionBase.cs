using System.Collections.Generic;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;

namespace SPMeta2.Standard.Definitions.Base
{
    public abstract class TemplateDefinitionBase : ContentPageDefinitionBase
    {
        public TemplateDefinitionBase()
        {

        }

        [ExpectUpdate]
        [ExpectValidation]
        public string Description { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public bool HiddenTemplate { get; set; }


    }
}
