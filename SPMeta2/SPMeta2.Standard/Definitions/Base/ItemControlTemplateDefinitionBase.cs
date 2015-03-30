using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Standard.Definitions.Base
{
    public abstract class ItemControlTemplateDefinitionBase : TemplateDefinitionBase
    {
        public ItemControlTemplateDefinitionBase()
        {
            TargetControlTypes = new List<string>();
        }

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
