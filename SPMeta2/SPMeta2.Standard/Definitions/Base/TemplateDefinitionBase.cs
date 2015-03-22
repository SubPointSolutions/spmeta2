using System.Collections.Generic;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;

namespace SPMeta2.Standard.Definitions.Base
{
    public abstract class TemplateDefinitionBase : ContentPageDefinitionBase
    {
        public TemplateDefinitionBase()
        {
            TargetControlTypes = new List<string>();
        }

        [ExpectUpdate]
        [ExpectValidation]
        public string Description { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public bool HiddenTemplate { get; set; }

        [ExpectUpdateAsTargetControlType]
        [ExpectValidation]
        public List<string> TargetControlTypes { get; set; }

        [ExpectUpdateAsUrl(Extension = "xslt")]
        [ExpectValidation]
        public string PreviewURL { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public string PreviewDescription { get; set; }
    }
}
