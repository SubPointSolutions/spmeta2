using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
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

    public class RegionalSettingsDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        public short AdjustHijriDays { get; set; }

        [ExpectValidation]
        public short AlternateCalendarType { get; set; }

        [ExpectValidation]
        public short CalendarType { get; set; }

        [ExpectValidation]
        public short Collation { get; set; }

        [ExpectValidation]
        public uint FirstDayOfWeek { get; set; }

        [ExpectValidation]
        public short FirstWeekOfYear { get; set; }

        [ExpectValidation]
        public uint LocaleId { get; set; }

        [ExpectValidation]
        public bool ShowWeeks { get; set; }

        [ExpectValidation]
        public bool Time24 { get; set; }

        [ExpectValidation]
        public short WorkDayEndHour { get; set; }

        [ExpectValidation]
        public short WorkDays { get; set; }

        [ExpectValidation]
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
