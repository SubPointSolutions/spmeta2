using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace SubPointSolutions.Docs.Code.Definitions
{
    public static class DocSiteFeatures
    {
        #region features

        public static FeatureDefinition SitePublisingInfrastructure = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure
           .Inherit(def =>
           {
               def.Enable = true;
           });

        public static FeatureDefinition DocumentSets = BuiltInSiteFeatures.DocumentSets
         .Inherit(def =>
         {
             def.Enable = true;
         });

        #endregion

        #region disable

        public static class Disable
        {
            public static FeatureDefinition MDS = BuiltInWebFeatures.MinimalDownloadStrategy
               .Inherit(def =>
               {
                   def.Enable = false;
               });
        }

        #endregion
    }
}
