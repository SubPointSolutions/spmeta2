using System.Collections.Generic;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;

namespace SPMeta2.Standard.Definitions.Base
{
    public abstract class TemplateDefinitionBase : PageDefinitionBase
    {
        public TemplateDefinitionBase()
        {
            Content = new byte[0];

            TargetControlTypes = new List<string>();
        }

        [ExpectUpdate]
        [ExpectValidation]
        public string Description { get; set; }

        [ExpectUpdateAsTargetControlType]
        [ExpectValidation]
        public bool HiddenTemplate { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        public List<string> TargetControlTypes { get; set; }

        public byte[] Content { get; set; }


        [ExpectUpdateAsFileName(Extension = "png")]
        [ExpectValidation]
        public string PreviewURL { get; set; }
        
        [ExpectUpdate]
        [ExpectValidation]
        public string PreviewDescription { get; set; }
    }
}
