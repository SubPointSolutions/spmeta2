using System;

using Microsoft.SharePoint.Client;

using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
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

            bool shouldUpdate;
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
                context.AddQuery(new ClientActionInvokeMethod(settings, "Update", null));
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
                context.AddQuery(new ClientActionSetProperty(settings, "AdjustHijriDays", definition.AdjustHijriDays.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "AlternateCalendarType")
                && definition.AlternateCalendarType.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "AlternateCalendarType", definition.AlternateCalendarType.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "CalendarType")
                && definition.CalendarType.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "CalendarType", definition.CalendarType.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "Collation")
                && definition.Collation.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "Collation", definition.Collation.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "FirstDayOfWeek")
                && definition.FirstDayOfWeek.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "FirstDayOfWeek", definition.FirstDayOfWeek.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "FirstWeekOfYear")
                && definition.FirstWeekOfYear.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "FirstWeekOfYear", definition.FirstWeekOfYear.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "LocaleId")
                && definition.LocaleId.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "LocaleId", definition.LocaleId.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "WorkDayStartHour")
                && definition.WorkDayStartHour.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "WorkDayStartHour", definition.WorkDayStartHour.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "WorkDayEndHour")
                && definition.WorkDayEndHour.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "WorkDayEndHour", definition.WorkDayEndHour.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "WorkDays")
                && definition.WorkDays.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "WorkDays", definition.WorkDays.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "ShowWeeks")
                && definition.ShowWeeks.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "ShowWeeks", definition.ShowWeeks.Value));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "Time24")
                && definition.Time24.HasValue)
            {
                context.AddQuery(new ClientActionSetProperty(settings, "Time24", definition.Time24.Value));
                shouldUpdate = true;
            }
        }

        #endregion
    }
}
