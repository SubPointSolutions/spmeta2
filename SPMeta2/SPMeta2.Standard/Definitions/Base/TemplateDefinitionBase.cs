using System.Collections.Generic;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Definitions.Base
{
    [DataContract]
    public abstract class TemplateDefinitionBase : ContentPageDefinitionBase
    {
        public TemplateDefinitionBase()
        {

        }

        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        [ExpectUpdate]
        [ExpectValidation]
        [DataMember]
        public bool HiddenTemplate { get; set; }


    }
}
