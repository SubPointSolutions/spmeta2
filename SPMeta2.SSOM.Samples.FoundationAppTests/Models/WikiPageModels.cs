using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class WikiPageModels
    {
        #region properties

        public static class SitePages
        {
            public static WikiPageDefinition TermsPage = new WikiPageDefinition
            {
                FileName = "Terms.aspx"
            };

            public static WikiPageDefinition ProducstPage = new WikiPageDefinition
            {
                FileName = "Products.aspx"
            };
        }

        #endregion
    }
}
