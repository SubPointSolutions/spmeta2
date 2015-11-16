using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.CSOM.Standard.Config
{
    public class MetadataNavigationKeyFilterConfig
    {
        public MetadataNavigationKeyFilterConfig(Guid fieldId)
        {
            this.FieldId = fieldId;
        }

        public Guid FieldId { get; set; }
    }
}
