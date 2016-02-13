using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;

namespace SPMeta2.Standard.BuiltInDefinitions
{
    public static class BuiltInPublishingPages
    {
        public static PublishingPageDefinition Default = new PublishingPageDefinition
        {
            FileName = "default.aspx"
        };

        public static PublishingPageDefinition PageNotFoundError = new PublishingPageDefinition
        {
            FileName = "PageNotFoundError.aspx"
        };
    }
}
