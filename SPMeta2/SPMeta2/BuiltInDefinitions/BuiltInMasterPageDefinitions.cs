using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;

namespace SPMeta2.BuiltInDefinitions
{
    public static class BuiltInMasterPageDefinitions
    {
        #region properties

        public static MasterPageSettingsDefinition Seattle = new MasterPageSettingsDefinition
        {
            SiteMasterPageUrl = "/_catalogs/masterpage/seattle.master",
            SystemMasterPageUrl = "/_catalogs/masterpage/seattle.master"
        };

        public static MasterPageSettingsDefinition Oslo = new MasterPageSettingsDefinition
        {
            SiteMasterPageUrl = "/_catalogs/masterpage/oslo.master",
            SystemMasterPageUrl = "/_catalogs/masterpage/oslo.master"
        };

        public static MasterPageSettingsDefinition Minimal = new MasterPageSettingsDefinition
        {
            SiteMasterPageUrl = "/_catalogs/masterpage/minimal.master",
            SystemMasterPageUrl = "/_catalogs/masterpage/minimal.master"
        };

        public static MasterPageSettingsDefinition V4 = new MasterPageSettingsDefinition
        {
            SiteMasterPageUrl = "/_catalogs/masterpage/v4.master",
            SystemMasterPageUrl = "/_catalogs/masterpage/v4.master"
        };

        #endregion
    }
}
