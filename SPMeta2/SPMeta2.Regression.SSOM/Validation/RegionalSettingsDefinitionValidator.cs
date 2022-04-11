using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class RegionalSettingsDefinitionValidator : RegionalSettingsModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RegionalSettingsDefinition>("model", value => value.RequireNotNull());


            var spObject = GetCurrentRegionalSettings(webModelHost.HostWeb);

            var assert = ServiceFactory.AssertService
                                      .NewAssert(definition, spObject)
                                            .ShouldNotBeNull(spObject);

            if (definition.CalendarType.HasValue)
                assert.ShouldBeEqual(m => m.CalendarType, o => o.CalendarType);
            else
                assert.SkipProperty(m => m.CalendarType, "CalendarType is null or empty");

            if (definition.AlternateCalendarType.HasValue)
                assert.ShouldBeEqual(m => m.AlternateCalendarType, o => o.AlternateCalendarType);
            else
                assert.SkipProperty(m => m.AlternateCalendarType, "AlternateCalendarType is null or empty");

            if (definition.AdjustHijriDays.HasValue)
                assert.ShouldBeEqual(m => m.AdjustHijriDays, o => o.AdjustHijriDays);
            else
                assert.SkipProperty(m => m.AdjustHijriDays, "AdjustHijriDays is null or empty");

            if (definition.Collation.HasValue)
                assert.ShouldBeEqual(m => m.Collation, o => o.Collation);
            else
                assert.SkipProperty(m => m.Collation, "Collation is null or empty");

            if (definition.WorkDayStartHour.HasValue)
                assert.ShouldBeEqual(m => m.WorkDayStartHour, o => o.WorkDayStartHour);
            else
                assert.SkipProperty(m => m.WorkDayStartHour, "WorkDayStartHour is null or empty");

            if (definition.WorkDayEndHour.HasValue)
                assert.ShouldBeEqual(m => m.WorkDayEndHour, o => o.WorkDayEndHour);
            else
                assert.SkipProperty(m => m.WorkDayEndHour, "WorkDayEndHour is null or empty");

            if (definition.WorkDays.HasValue)
                assert.ShouldBeEqual(m => m.WorkDays, o => o.WorkDays);
            else
                assert.SkipProperty(m => m.WorkDays, "WorkDays is null or empty");

            if (definition.Time24.HasValue)
                assert.ShouldBeEqual(m => m.Time24, o => o.Time24);
            else
                assert.SkipProperty(m => m.Time24, "Time24 is null or empty");

            if (definition.ShowWeeks.HasValue)
                assert.ShouldBeEqual(m => m.ShowWeeks, o => o.ShowWeeks);
            else
                assert.SkipProperty(m => m.ShowWeeks, "ShowWeeks is null or empty");

            if (definition.FirstDayOfWeek.HasValue)
                assert.ShouldBeEqual(m => m.FirstWeekOfYear, o => o.FirstWeekOfYear);
            else
                assert.SkipProperty(m => m.FirstWeekOfYear, "FirstWeekOfYear is null or empty");

            if (definition.FirstDayOfWeek.HasValue)
                assert.ShouldBeEqual(m => m.FirstDayOfWeek, o => o.FirstDayOfWeek);
            else
                assert.SkipProperty(m => m.FirstDayOfWeek, "FirstDayOfWeek is null or empty");

            if (definition.LocaleId.HasValue)
                assert.ShouldBeEqual(m => m.LocaleId, o => o.LocaleId);
            else
                assert.SkipProperty(m => m.LocaleId, "LocaleId is null or empty");

            if (definition.TimeZoneId.HasValue)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.TimeZoneId);
                    var isValid = s.TimeZoneId == d.TimeZone.ID;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.TimeZoneId, "TimeZoneId is not set. Skipping.");
            }
        }

        #endregion
    }
}
