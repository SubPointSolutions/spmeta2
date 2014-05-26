namespace SPMeta2.Definitions
{
    public class PublishingPageDefinition : PageDefinitionBase
    {
        #region properties

        public string PageLayoutFileName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] FileName:[{1}] FolderUrl:[{2}] PageLayoutFileName:[{3}]",
                new[] { Title, FileName, FolderUrl, PageLayoutFileName });
        }

        #endregion
    }
}
