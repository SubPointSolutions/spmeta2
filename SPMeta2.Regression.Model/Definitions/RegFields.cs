using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegFields
    {
        #region properties

        public static FieldDefinition TextField = new FieldDefinition
        {
            Id = new Guid("{929D9BE4-D97B-4840-8D9D-A4A8A2EE7D18}"),
            Title = "Test Text",
            InternalName = "spmeta2Reg_TestText",
            Description = String.Empty,
            Group = "SPMeta2 Regression test",
            FieldType = BuiltInFieldTypes.Text
        };

        #endregion
    }
}
