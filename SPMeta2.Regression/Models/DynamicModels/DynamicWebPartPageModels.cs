using System;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Models.DynamicModels
{
    public static class DynamicWebPartPageModels
    {
        #region properties

        public static class SitePages
        {
            #region webpart pages

            public static WebPartPageDefinition WebPartPage1 = GetWebPartPageTemplate(1);
            public static WebPartPageDefinition WebPartPage2 = GetWebPartPageTemplate(2);
            public static WebPartPageDefinition WebPartPage3 = GetWebPartPageTemplate(3);
            public static WebPartPageDefinition WebPartPage4 = GetWebPartPageTemplate(4);
            public static WebPartPageDefinition WebPartPage5 = GetWebPartPageTemplate(5);
            public static WebPartPageDefinition WebPartPage6 = GetWebPartPageTemplate(6);
            public static WebPartPageDefinition WebPartPage7 = GetWebPartPageTemplate(7);
            public static WebPartPageDefinition WebPartPage8 = GetWebPartPageTemplate(8);

            public static WebPartPageDefinition GetWebPartPageTemplate(int type)
            {
                var id = Guid.NewGuid().ToString("N");

                return new WebPartPageDefinition
                {
                    Title = string.Format("WebPartPage{0}{1}", type, id),
                    FileName = string.Format("WebPartPage{0}{1}", type, id),
                    PageLayoutTemplate = type,
                    NeedOverride = true
                };
            }

            #endregion

            #region wiki pages

            public static WikiPageDefinition WikiPage1 = GetWikiPageTemplate();
            public static WikiPageDefinition WikiPage2 = GetWikiPageTemplate();
            public static WikiPageDefinition WikiPage3 = GetWikiPageTemplate();
            public static WikiPageDefinition WikiPage4 = GetWikiPageTemplate();
            public static WikiPageDefinition WikiPage5 = GetWikiPageTemplate();

            public static WikiPageDefinition GetWikiPageTemplate()
            {
                var id = Guid.NewGuid().ToString("N");

                return new WikiPageDefinition
                {
                    Title = string.Format("WikiPage{0}", id),
                    FileName = string.Format("WikiPage{0}", id),
                    NeedOverride = true
                };
            }

            #endregion

            #region publishing pages

            public static PublishingPageDefinition PublishingArticleLeft = GetPublishingPageTemplate("ArticleLeft.aspx");
            public static PublishingPageDefinition PublishingArticleRight = GetPublishingPageTemplate("ArticleRight.aspx");
            public static PublishingPageDefinition PublishingProjectPage = GetPublishingPageTemplate("ProjectPage.aspx");
            public static PublishingPageDefinition PublishingBlankWebPartPage = GetPublishingPageTemplate("BlankWebPartPage.aspx");
            public static PublishingPageDefinition PublishingEnterpriseWikiPage = GetPublishingPageTemplate("EnterpriseWiki.aspx");

            public static PublishingPageDefinition GetPublishingPageTemplate(string pageLayoutFileName)
            {
                var id = Guid.NewGuid().ToString("N");

                return new PublishingPageDefinition
                {
                    Title = string.Format("PublishingPage{0}", id),
                    FileName = string.Format("PublishingPage{0}", id),
                    PageLayoutFileName = pageLayoutFileName,
                    NeedOverride = true
                };
            }

            #endregion
        }

        #endregion
    }
}