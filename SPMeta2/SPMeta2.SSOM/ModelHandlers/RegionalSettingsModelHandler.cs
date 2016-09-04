using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class RegionalSettingsModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(RegionalSettingsDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<RegionalSettingsDefinition>("model", value => value.RequireNotNull());

            DeployRegionalSettings(modelHost, webModelHost.HostWeb, definition);
        }

        private void DeployRegionalSettings(object modelHost, SPWeb web, RegionalSettingsDefinition definition)
        {
            var settings = GetCurrentRegionalSettings(web);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = settings,
                ObjectType = typeof(SPRegionalSettings),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            MapRegionalSettings(settings, definition);

            web.RegionalSettings = settings;
            web.Update();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = settings,
                ObjectType = typeof(SPRegionalSettings),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });
        }

        protected SPRegionalSettings GetCurrentRegionalSettings(SPWeb web)
        {
            return new SPRegionalSettings(web);
        }

        private void MapRegionalSettings(SPRegionalSettings settings, RegionalSettingsDefinition definition)
        {
            if (definition.AdjustHijriDays.HasValue)
                settings.AdjustHijriDays = definition.AdjustHijriDays.Value;

            if (definition.AlternateCalendarType.HasValue)
                settings.AlternateCalendarType = definition.AlternateCalendarType.Value;

            if (definition.CalendarType.HasValue)
                settings.CalendarType = definition.CalendarType.Value;

            if (definition.Collation.HasValue)
                settings.Collation = definition.Collation.Value;

            if (definition.FirstDayOfWeek.HasValue)
                settings.FirstDayOfWeek = definition.FirstDayOfWeek.Value;

            if (definition.FirstWeekOfYear.HasValue)
                settings.FirstWeekOfYear = definition.FirstWeekOfYear.Value;

            if (definition.LocaleId.HasValue)
                settings.LocaleId = definition.LocaleId.Value;

            if (definition.WorkDayStartHour.HasValue)
                settings.WorkDayStartHour = definition.WorkDayStartHour.Value;

            if (definition.WorkDayEndHour.HasValue)
                settings.WorkDayEndHour = definition.WorkDayEndHour.Value;

            if (definition.WorkDays.HasValue)
                settings.WorkDays = definition.WorkDays.Value;

            if (definition.ShowWeeks.HasValue)
                settings.ShowWeeks = definition.ShowWeeks.Value;

            if (definition.Time24.HasValue)
                settings.Time24 = definition.Time24.Value;

            if (definition.LocaleId.HasValue)
                settings.LocaleId = definition.LocaleId.Value;

            if (definition.TimeZoneId.HasValue)
            {
                var targetZone = settings.TimeZones
                    .OfType<SPTimeZone>()
                    .FirstOrDefault(z => z.ID == definition.TimeZoneId.Value);

                if (targetZone == null)
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find TimeZone by ID:[{0}]", definition.TimeZoneId));
                }

                settings.TimeZone.ID = definition.TimeZoneId.Value;
            }
        }

        #endregion
    }
}
