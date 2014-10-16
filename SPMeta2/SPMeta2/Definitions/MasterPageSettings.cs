namespace SPMeta2.Definitions
{
    public class MasterPageSettingsDefinition : DefinitionBase
    {
        #region properties

        public string SiteMasterPageUrl { get; set; }
        public bool? SiteMasterPageInheritFromMaster { get; set; }

        public string SystemMasterPageUrl { get; set; }
        public string SystemMasterPageInheritFromMaster { get; set; }

        #endregion
    }
}
