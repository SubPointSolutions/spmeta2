using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public class PublishingPageModels
    {
        #region properties

        public static PublishingPageDefinition AboutUsPage = new PublishingPageDefinition
        {
            Title = "About us",
            FileName = "About us page.aspx",
            PageLayoutFileName = "ArticleLeft.aspx",
        };

        public static PublishingPageDefinition AboutServicesPage = new PublishingPageDefinition
        {
            Title = "Services",
            FileName = "About Services.aspx",
            PageLayoutFileName = "ArticleRight.aspx",
            FolderUrl = "services/about"
        };

        #endregion
    }
}
