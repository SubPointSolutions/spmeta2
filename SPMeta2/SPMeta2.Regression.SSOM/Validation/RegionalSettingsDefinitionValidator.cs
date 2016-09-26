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
                                            .ShouldNotBeNull(spObject)
                                            .ShouldBeEqual(m => m.AdjustHijriDays, o => o.AdjustHijriDays)
                                            .ShouldBeEqual(m => m.AlternateCalendarType, o => o.AlternateCalendarType)
                                            .ShouldBeEqual(m => m.CalendarType, o => o.CalendarType)
                                            .ShouldBeEqual(m => m.Collation, o => o.Collation)
                                            .ShouldBeEqual(m => m.FirstDayOfWeek, o => o.FirstDayOfWeek)
                                            .ShouldBeEqual(m => m.FirstWeekOfYear, o => o.FirstWeekOfYear)
                                            //.ShouldBeEqual(m => m.LocaleId, o => o.LocaleId)
                                            .ShouldBeEqual(m => m.WorkDayEndHour, o => o.WorkDayEndHour)
                                            .ShouldBeEqual(m => m.WorkDayStartHour, o => o.WorkDayStartHour)
                                            .ShouldBeEqual(m => m.WorkDays, o => o.WorkDays)
                                            .ShouldBeEqual(m => m.ShowWeeks, o => o.ShowWeeks)
                                            .ShouldBeEqual(m => m.Time24, o => o.Time24);

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
