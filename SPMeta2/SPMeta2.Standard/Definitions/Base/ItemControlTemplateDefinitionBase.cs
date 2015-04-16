using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes.Regression;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Definitions.Base
{
    [DataContract]
    public abstract class ItemControlTemplateDefinitionBase : TemplateDefinitionBase
    {
        public ItemControlTemplateDefinitionBase()
        {
            TargetControlTypes = new List<string>();
        }

        [ExpectUpdateAsTargetControlType]
        [ExpectValidation]
        [DataMember]
        public List<string> TargetControlTypes { get; set; }

        [ExpectUpdateAsUrl(Extension = "xslt")]
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string PreviewURL { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string PreviewDescription { get; set; }
    }
}
