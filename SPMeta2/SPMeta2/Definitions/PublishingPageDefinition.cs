namespace SPMeta2.Definitions
{

    /// <summary>
    /// Allows to define and deploy SharePoint publishing page.
    /// </summary>
    public class PublishingPageDefinition : PageDefinitionBase
    {
        #region properties

        /// <summary>
        /// Page layout name of the target publishing page.
        /// </summary>
        public string PageLayoutFileName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] FileName:[{1}] PageLayoutFileName:[{2}]",
                new[] { Title, FileName, PageLayoutFileName });
        }

        #endregion
    }
}
