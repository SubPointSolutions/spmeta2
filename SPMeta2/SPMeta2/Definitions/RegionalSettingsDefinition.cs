using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using System.Runtime.Serialization;

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
    public class RegionalSettingsDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        public short AdjustHijriDays { get; set; }

        [ExpectValidation]
        [DataMember]
        public short AlternateCalendarType { get; set; }

        [ExpectValidation]
        [DataMember]
        public short CalendarType { get; set; }

        [ExpectValidation]
        [DataMember]
        public short Collation { get; set; }

        [ExpectValidation]
        [DataMember]
        public uint FirstDayOfWeek { get; set; }

        [ExpectValidation]
        [DataMember]
        public short FirstWeekOfYear { get; set; }

        [ExpectValidation]
        [DataMember]
        public uint LocaleId { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool ShowWeeks { get; set; }

        [ExpectValidation]
        [DataMember]
        public bool Time24 { get; set; }

        [ExpectValidation]
        [DataMember]
        public short WorkDayEndHour { get; set; }

        [ExpectValidation]
        [DataMember]
        public short WorkDays { get; set; }

        [ExpectValidation]
        [DataMember]
        public short WorkDayStartHour { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<RegionalSettingsDefinition>(this)
                          .AddPropertyValue(p => p.AdjustHijriDays)
                          .AddPropertyValue(p => p.AlternateCalendarType)
                          .AddPropertyValue(p => p.CalendarType)
                          .AddPropertyValue(p => p.Collation)
                          .AddPropertyValue(p => p.LocaleId)
                          .AddPropertyValue(p => p.ShowWeeks)
                          .AddPropertyValue(p => p.Time24)
                          .AddPropertyValue(p => p.WorkDayStartHour)
                          .AddPropertyValue(p => p.WorkDayEndHour)
                          .AddPropertyValue(p => p.WorkDays)
                          .ToString();
        }

        #endregion
    }
}
