using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy regional setting for target web site.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPRegionalSettings", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.RegionalSettings", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]
    [SingletonIdentity]

    [ParentHostCapability(typeof(WebDefinition))]
    public class RegionalSettingsDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 1, MaxValue = 5)]
        [ExpectNullable]
        public short? AdjustHijriDays { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 0, MaxValue = 16)]
        [ExpectNullable]
        public short? AlternateCalendarType { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 0, MaxValue = 16)]
        [ExpectNullable]
        public short? CalendarType { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 0, MaxValue = 38)]
        [ExpectNullable]
        public short? Collation { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        [ExpectUpdateAsIntRange(MinValue = 1, MaxValue = 4)]
        public uint? FirstDayOfWeek { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 0, MaxValue = 2)]
        [ExpectNullable]
        public short? FirstWeekOfYear { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsLCID]
        [ExpectNullable]
        public uint? LocaleId { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdate]
        [ExpectNullable]
        public bool? ShowWeeks { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdate]
        [ExpectNullable]
        public bool? Time24 { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 5, MaxValue = 8)]
        [ExpectNullable]
        public short? WorkDayEndHour { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 1, MaxValue = 5)]
        [ExpectNullable]
        public short? WorkDays { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 1, MaxValue = 4)]
        [ExpectNullable]
        public short? WorkDayStartHour { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 5, MaxValue = 10)]
        public ushort? TimeZoneId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("TimeZoneId", TimeZoneId)
                          .AddRawPropertyValue("AdjustHijriDays", AdjustHijriDays)
                          .AddRawPropertyValue("AlternateCalendarType", AlternateCalendarType)
                          .AddRawPropertyValue("CalendarType", CalendarType)
                          .AddRawPropertyValue("Collation", Collation)
                          .AddRawPropertyValue("LocaleId", LocaleId)
                          .AddRawPropertyValue("ShowWeeks", ShowWeeks)
                          .AddRawPropertyValue("Time24", Time24)
                          .AddRawPropertyValue("WorkDayStartHour", WorkDayStartHour)
                          .AddRawPropertyValue("WorkDayEndHour", WorkDayEndHour)
                          .AddRawPropertyValue("WorkDays", WorkDays)
                          .ToString();
        }

        #endregion
    }
}
