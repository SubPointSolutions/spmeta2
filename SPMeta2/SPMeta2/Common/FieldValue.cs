using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Common
{
    public class FieldValue
    {
        public string FieldName { get; set; }
        public Guid? FieldId { get; set; }

        public object Value { get; set; }
    }
}
