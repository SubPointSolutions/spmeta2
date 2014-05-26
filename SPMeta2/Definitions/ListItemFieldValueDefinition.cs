using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Definitions
{
    public class ListItemFieldValueDefinition : DefinitionBase
    {
        #region properties

        public string FieldName { get; set; }
        public Guid? FieldId { get; set; }

        public object Value { get; set; }

        #endregion
    }
}
