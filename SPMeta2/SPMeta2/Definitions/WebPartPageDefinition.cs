namespace SPMeta2.Definitions
{
    public class WebPartPageDefinition : PageDefinitionBase
    {
        #region contructors

        public WebPartPageDefinition()
        {
            NeedOverride = true;
        }

        #endregion

        #region properties

        public int PageLayoutTemplate { get; set; }
        public string CustomPageLayout { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("{0} PageLayoutTemplate:[{1}]", base.ToString(), PageLayoutTemplate);
        }

        #endregion
    }
}
