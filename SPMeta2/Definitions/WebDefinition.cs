namespace SPMeta2.Definitions
{
    public class WebDefinition : DefinitionBase
    {
        #region contructors

        public WebDefinition()
        {
            Url = "/";
        }

        #endregion

        #region properties

        public string Title { get; set; }
        public string Description { get; set; }

        public uint LCID { get; set; }

        public bool UseUniquePermission { get; set; }
        public bool ConvertIfThere { get; set; }

        public string Url { get; set; }
        public string WebTemplate { get; set; }
        public string CustomWebTemplate { get; set; }

        #endregion
    }
}
