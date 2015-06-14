using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Services.Impl
{
    public class DefaultWebPartChromeTypesConvertService : WebPartChromeTypesConvertService
    {
        #region constructors

        public DefaultWebPartChromeTypesConvertService()
        {
            MapFromFrameTypesToPartChromeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            MapFromPartChromeTypesToFrameTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            MapFromFrameTypesToPartChromeTypes.Add("Standard", "TitleAndBorder");
            MapFromFrameTypesToPartChromeTypes.Add("TitleBarOnly", "TitleOnly");

            MapFromPartChromeTypesToFrameTypes.Add("TitleOnly", "TitleBarOnly");
            MapFromPartChromeTypesToFrameTypes.Add("TitleAndBorder", "Standard");
        }

        #endregion

        #region properties

        protected static List<string> PartChromeTypes = new List<string>
        {
            "Default",
            "TitleAndBorder",
            "None",
            "TitleOnly",
            "BorderOnly"
        };

        protected static List<string> FrameTypes = new List<string>
        {
           "None",
		   "Standard",
		   "TitleBarOnly",
		   "Default",
		   "BorderOnly"
        };

        protected Dictionary<string, string> MapFromFrameTypesToPartChromeTypes { get; set; }
        protected Dictionary<string, string> MapFromPartChromeTypesToFrameTypes { get; set; }

        #endregion

        #region methods

        public override string NormilizeValueToPartChromeTypes(string value)
        {
            if (MapFromFrameTypesToPartChromeTypes.ContainsKey(value))
                return MapFromFrameTypesToPartChromeTypes[value];

            return value;
        }

        public override string NormilizeValueToFrameTypes(string value)
        {
            if (MapFromPartChromeTypesToFrameTypes.ContainsKey(value))
                return MapFromPartChromeTypesToFrameTypes[value];

            return value;
        }

        #endregion
    }
}
