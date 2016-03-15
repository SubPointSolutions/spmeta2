using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.CSOM.Standard.Config
{
    public class MetadataNavigationHierarchyConfig
    {
        public MetadataNavigationHierarchyConfig(Guid fieldId)
        {
            this.FieldId = fieldId;
        }

        public Guid FieldId { get; set; }
    }
}
