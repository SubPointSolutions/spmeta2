using System;
using System.Linq;

using Microsoft.SharePoint.Client;

using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class RegionalSettingsModelHandler : CSOMModelHandlerBase
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

        private void DeployRegionalSettings(object modelHost, Web web, RegionalSettingsDefinition definition)
        {
            var context = web.Context;

            var settings = GetCurrentRegionalSettings(web);
            var shouldUpdate = SupportSetters(settings);

            if (shouldUpdate && definition.TimeZoneId.HasValue)
            {
                // pre-load TimeZones for the further lookup
                context.Load(settings);
                context.Load(settings, s => s.TimeZones);

                context.ExecuteQueryWithTrace();
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = settings,
                ObjectType = typeof(RegionalSettings),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            MapRegionalSettings(context, settings, definition, out shouldUpdate);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = settings,
                ObjectType = typeof(RegionalSettings),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (shouldUpdate)
            {
                ClientRuntimeQueryService.InvokeMethod(settings, "Update");
                context.ExecuteQueryWithTrace();
            }
        }

        protected RegionalSettings GetCurrentRegionalSettings(Web web)
        {
            return web.RegionalSettings;
        }

        protected virtual bool SupportSetters(RegionalSettings settings)
        {
            // should have at least one setter
            // Update() is there, but setters aren't

            var supportedRuntime = ReflectionUtils.HasMethod(settings, "Update")
                && ReflectionUtils.HasPropertyPublicSetter(settings, "AdjustHijriDays");

            if (!supportedRuntime)
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                        "CSOM runtime doesn't have RegionalSettings.Update() methods support. Update CSOM runtime to a new version. RegionalSettings provision is skipped");
            }

            return supportedRuntime;
        }

        protected virtual void MapRegionalSettings(ClientRuntimeContext context, RegionalSettings settings, RegionalSettingsDefinition definition,
            out bool shouldUpdate)
        {
            shouldUpdate = false;

            if (!SupportSetters(settings))
                return;

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "AdjustHijriDays")
                && definition.AdjustHijriDays.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "AdjustHijriDays", definition.AdjustHijriDays.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "AlternateCalendarType")
                && definition.AlternateCalendarType.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "AlternateCalendarType", definition.AlternateCalendarType.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "CalendarType")
                && definition.CalendarType.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "CalendarType", definition.CalendarType.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "Collation")
                && definition.Collation.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "Collation", definition.Collation.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "FirstDayOfWeek")
                && definition.FirstDayOfWeek.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "FirstDayOfWeek", definition.FirstDayOfWeek.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "FirstWeekOfYear")
                && definition.FirstWeekOfYear.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "FirstWeekOfYear", definition.FirstWeekOfYear.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "LocaleId")
                && definition.LocaleId.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "LocaleId", definition.LocaleId.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "WorkDayStartHour")
                && definition.WorkDayStartHour.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "WorkDayStartHour", definition.WorkDayStartHour.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "WorkDayEndHour")
                && definition.WorkDayEndHour.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "WorkDayEndHour", definition.WorkDayEndHour.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "WorkDays")
                && definition.WorkDays.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "WorkDays", definition.WorkDays.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "ShowWeeks")
                && definition.ShowWeeks.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "ShowWeeks", definition.ShowWeeks.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "Time24")
                && definition.Time24.HasValue)
            {
                ClientRuntimeQueryService.SetProperty(settings, "Time24", definition.Time24.Value);
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "TimeZone")
                && definition.TimeZoneId.HasValue)
            {
                var targetZone = settings.TimeZones
                    .ToArray()
                    .FirstOrDefault(z => z.Id == definition.TimeZoneId.Value);

                if (targetZone == null)
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find TimeZone by ID:[{0}]", definition.TimeZoneId));
                }

                ClientRuntimeQueryService.SetProperty(settings, "TimeZone", targetZone);
                shouldUpdate = true;
            }
        }

        #endregion
    }
}
