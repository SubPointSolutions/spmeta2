using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientRegionalSettingsDefinitionValidator : RegionalSettingsModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RegionalSettingsDefinition>("model", value => value.RequireNotNull());

            var context = webModelHost.HostClientContext;
            var spObject = GetCurrentRegionalSettings(webModelHost.HostWeb);

            context.Load(spObject);
            context.Load(spObject, s => s.TimeZone);
            context.Load(spObject, s => s.TimeZones);

            context.ExecuteQueryWithTrace();

            var skippingMessage = "CSOM does not support operations on RegionalSettings. Skipping.";

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject);

            if (!SupportSetters(spObject))
            {
                assert
                    .SkipProperty(m => m.AdjustHijriDays, skippingMessage)
                    .SkipProperty(m => m.AlternateCalendarType, skippingMessage)
                    .SkipProperty(m => m.CalendarType, skippingMessage)
                    .SkipProperty(m => m.Collation, skippingMessage)
                    .SkipProperty(m => m.FirstDayOfWeek, skippingMessage)
                    .SkipProperty(m => m.FirstWeekOfYear, skippingMessage)
                    .SkipProperty(m => m.LocaleId, skippingMessage)
                    .SkipProperty(m => m.WorkDayEndHour, skippingMessage)
                    .SkipProperty(m => m.WorkDayStartHour, skippingMessage)
                    .SkipProperty(m => m.WorkDays, skippingMessage)
                    .SkipProperty(m => m.ShowWeeks, skippingMessage)
                    .SkipProperty(m => m.Time24, skippingMessage)
                    .SkipProperty(m => m.TimeZoneId, skippingMessage);
            }
            else
            {
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
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.WorkDayStartHour);
                        // WHAT?? why minutes??
                        var isValid = s.WorkDayStartHour == d.WorkDayStartHour / 60;

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
                    assert.SkipProperty(m => m.WorkDayStartHour, "WorkDayStartHour is null or empty");

                if (definition.WorkDayEndHour.HasValue)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.WorkDayEndHour);
                        // WHAT?? why minutes??
                        var isValid = s.WorkDayEndHour == d.WorkDayEndHour / 60;

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
                        var isValid = s.TimeZoneId == d.TimeZone.Id;

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
        }

        #endregion
    }
}
