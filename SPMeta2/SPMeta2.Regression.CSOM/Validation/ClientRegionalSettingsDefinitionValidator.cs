using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
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
            
            var spObject = GetCurrentRegionalSettings(webModelHost.HostWeb);

            var skippingMessage = "CSOM does not support operations on RegionalSettings. Skipping.";

            var assert = ServiceFactory.AssertService
                                     .NewAssert(definition, spObject)
                                           .ShouldNotBeNull(spObject)
                                           //.ShouldBeEqual(m => m.AdjustHijriDays, o => o.AdjustHijriDays)
                                           //.ShouldBeEqual(m => m.AlternateCalendarType, o => o.AlternateCalendarType)
                                           //.ShouldBeEqual(m => m.CalendarType, o => o.CalendarType)
                                           //.ShouldBeEqual(m => m.Collation, o => o.Collation)
                                           //.ShouldBeEqual(m => m.FirstDayOfWeek, o => o.FirstDayOfWeek)
                                           //.ShouldBeEqual(m => m.FirstWeekOfYear, o => o.FirstWeekOfYear)
                                           //.ShouldBeEqual(m => m.LocaleId, o => o.LocaleId)
                                           //.ShouldBeEqual(m => m.WorkDayEndHour, o => o.WorkDayEndHour)
                                           //.ShouldBeEqual(m => m.WorkDayStartHour, o => o.WorkDayStartHour)
                                           //.ShouldBeEqual(m => m.WorkDays, o => o.WorkDays)
                                           //.ShouldBeEqual(m => m.ShowWeeks, o => o.ShowWeeks)
                                           //.ShouldBeEqual(m => m.Time24, o => o.Time24);
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
                                           .SkipProperty(m => m.Time24,skippingMessage);

        }

        #endregion
    }
}
