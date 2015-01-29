namespace SPMeta2.Standard.Enumerations
{
    /// <summary>
    /// Out of the box SharePoint publishing field types.
    /// Correcponds to fldtypes_publishing.xml in TEMPLATE\XML folder.
    /// </summary>
    public static class BuiltInPublishingFieldTypes
    {
        public static readonly string HTML = "HTML";
        public static readonly string Image = "Image";

        public static readonly string Link = "Link";
        public static readonly string SummaryLinks = "SummaryLinks";

        public static readonly string LayoutVariationsField = "LayoutVariationsField";
        public static readonly string ContentTypeIdFieldType = "ContentTypeIdFieldType";

        public static readonly string PublishingScheduleStartDateFieldType = "PublishingScheduleStartDateFieldType";
        public static readonly string PublishingScheduleEndDateFieldType = "PublishingScheduleEndDateFieldType";

        public static readonly string MediaFieldType = "MediaFieldType";
        public static readonly string UserAgentSubstringsFieldType = "UserAgentSubstringsFieldType";


        public static readonly string ChannelAliasFieldType = "ChannelAliasFieldType";
        public static readonly string PublishingCatalogSourceFieldType = "PublishingCatalogSourceFieldType";
    }
}
