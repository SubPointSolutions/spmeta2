using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public static class WebPartPageModels
    {
        #region properties

        public static class SitePages
        {
            public static WebPartPageDefinition Page1 = GetWebPartPageTemplate(1);
            public static WebPartPageDefinition Page2 = GetWebPartPageTemplate(2);
            public static WebPartPageDefinition Page3 = GetWebPartPageTemplate(3);
            public static WebPartPageDefinition Page4 = GetWebPartPageTemplate(4);
            public static WebPartPageDefinition Page5 = GetWebPartPageTemplate(5);
            public static WebPartPageDefinition Page6 = GetWebPartPageTemplate(6);
            public static WebPartPageDefinition Page7 = GetWebPartPageTemplate(7);
            public static WebPartPageDefinition Page8 = GetWebPartPageTemplate(8);

            public static WebPartPageDefinition GetWebPartPageTemplate(int type)
            {
                return new WebPartPageDefinition
                {
                    Title = string.Format("Page{0}", type),
                    FileName = string.Format("Page{0}", type),
                    PageLayoutTemplate = type,
                    NeedOverride = true
                };
            }
        }

        #endregion
    }
}
