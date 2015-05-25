using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;

namespace SPMeta2.Standard.Definitions.Base
{
    public abstract class ReusableItemDefinitionBase : ListItemDefinition
    {
        #region constructors

        public ReusableItemDefinitionBase()
        {
            Overwrite = true;
            AutomaticUpdate = true;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        public string Comments { get; set; }

        [ExpectValidation]
        [DataMember]
        public string ContentCategory { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool AutomaticUpdate { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool ShowInDropDownMenu { get; set; }

        #endregion
    }
}
