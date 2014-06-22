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
            return string.Format("Title:[{0}] FileName:[{1}] PageLayoutFileName:[{2}]",
                new[] { Title, FileName, PageLayoutFileName });
        }

        #endregion
    }
}
