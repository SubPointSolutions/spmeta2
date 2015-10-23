using System;
using System.Collections.Generic;

namespace SPMeta2.Services.Impl
{
    public class DefaultListViewScopeTypesConvertService : ListViewScopeTypesConvertService
    {
        #region constructors

        public DefaultListViewScopeTypesConvertService()
        {
            MapToSSOMTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            MapToCSOMTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            MapToCSOMTypes.Add("Default", "DefaultValue");
            MapToSSOMTypes.Add("DefaultValue", "Default");
        }

        #endregion

        #region properties

        protected Dictionary<string, string> MapToSSOMTypes { get; set; }
        protected Dictionary<string, string> MapToCSOMTypes { get; set; }

        #endregion

        #region methods

        public override string NormilizeValueToSSOMType(string value)
        {
            if (MapToSSOMTypes.ContainsKey(value))
                return MapToSSOMTypes[value];

            return value;
        }

        public override string NormilizeValueToCSOMType(string value)
        {
            if (MapToCSOMTypes.ContainsKey(value))
                return MapToCSOMTypes[value];

            return value;
        }

        #endregion
    }
}
