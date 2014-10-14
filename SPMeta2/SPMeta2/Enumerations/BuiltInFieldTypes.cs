using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Out of the box SharePoint field types.
    /// </summary>
    public static class BuiltInFieldTypes
    {
        public static readonly string Invalid = "Invalid";
        public static readonly string Integer = "Integer";
        public static readonly string Text = "Text";
        public static readonly string Note = "Note";
        public static readonly string DateTime = "DateTime";
        public static readonly string Counter = "Counter";
        public static readonly string Choice = "Choice";
        public static readonly string Lookup = "Lookup";
        public static readonly string Boolean = "Boolean";
        public static readonly string Number = "Number";
        public static readonly string Currency = "Currency";
        public static readonly string URL = "URL";
        public static readonly string Computed = "Computed";
        public static readonly string Threading = "Threading";
        public static readonly string Guid = "Guid";
        public static readonly string MultiChoice = "MultiChoice";
        public static readonly string GridChoice = "GridChoice";
        public static readonly string Calculated = "Calculated";
        public static readonly string File = "File";
        public static readonly string Attachments = "Attachments";
        public static readonly string User = "User";
        public static readonly string Recurrence = "Recurrence";
        public static readonly string CrossProjectLink = "CrossProjectLink";
        public static readonly string ModStat = "ModStat";
        public static readonly string Error = "Error";
        public static readonly string ContentTypeId = "ContentTypeId";
        public static readonly string PageSeparator = "PageSeparator";
        public static readonly string ThreadIndex = "ThreadIndex";
        public static readonly string WorkflowStatus = "WorkflowStatus";
        public static readonly string AllDayEvent = "AllDayEvent";
        public static readonly string WorkflowEventType = "WorkflowEventType";
        public static readonly string Geolocation = "Geolocation";
        public static readonly string OutcomeChoice = "OutcomeChoice";
        public static readonly string MaxItems = "MaxItems";

        public static readonly string TaxonomyFieldType = "TaxonomyFieldType";
        public static readonly string TaxonomyFieldTypeMulti = "TaxonomyFieldTypeMulti";

        public static readonly string BusinessData = "BusinessData";

        public static string Overbook = "Overbook";
        public static string Whereabout = "Whereabout";
        public static string UserMulti = "UserMulti";
        public static string RelatedItems = "RelatedItems";
        public static string CallTo = "CallTo";
        public static string FreeBusy = "FreeBusy";
        public static string Likes = "Likes";
        public static string RatingCount = "RatingCount";
        public static string LookupMulti = "UserMulti";
        public static string HTML = "HTML";
        public static string TargetTo = "TargetTo";
        public static string SummaryLinks = "SummaryLinks";
        public static string Image = "Image";
        public static string LayoutVariationsField = "LayoutVariationsField";
        public static string Facilities = "Facilities";
        public static string AverageRating = "AverageRating";
        public static string ExemptField = "ExemptField";
        public static string ContactInfo = "ContactInfo";
        public static string ContentTypeIdFieldType = "ContentTypeId";
        public static string PublishingScheduleEndDateFieldType = DateTime;
        public static string PublishingScheduleStartDateFieldType = DateTime;
    }
}
