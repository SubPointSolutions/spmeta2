namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint web part page.
    /// </summary>
    public class WebPartPageDefinition : PageDefinitionBase
    {
        #region contructors

        public WebPartPageDefinition()
        {
            NeedOverride = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Out of the box web part page layout id.
        /// 
        /// BuiltInWebpartPageTemplateId class can be used to utilize out of the box SharePoint web part page templates.
        /// </summary>
        public int PageLayoutTemplate { get; set; }

        /// <summary>
        /// Custom web part page layout content.
        /// </summary>
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
