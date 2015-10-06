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

        private static void MapRegionalSettings(ClientRuntimeContext context, RegionalSettings settings, RegionalSettingsDefinition definition, out bool shouldUpdate)
        {
            shouldUpdate = false;

            if (!ReflectionUtils.HasMethod(settings, "Update"))
                return;

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "AdjustHijriDays"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "AdjustHijriDays", definition.AdjustHijriDays));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "AlternateCalendarType"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "AlternateCalendarType", definition.AlternateCalendarType));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "CalendarType"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "CalendarType", definition.CalendarType));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "Collation"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "Collation", definition.Collation));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "FirstDayOfWeek"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "FirstDayOfWeek", definition.FirstDayOfWeek));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "FirstWeekOfYear"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "FirstWeekOfYear", definition.FirstWeekOfYear));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "LocaleId"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "LocaleId", definition.LocaleId));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "WorkDayStartHour"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "WorkDayStartHour", definition.WorkDayStartHour));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "WorkDayEndHour"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "WorkDayEndHour", definition.WorkDayEndHour));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "WorkDays"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "WorkDays", definition.WorkDays));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "ShowWeeks"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "ShowWeeks", definition.ShowWeeks));
                shouldUpdate = true;
            }

            if (ReflectionUtils.HasPropertyPublicSetter(settings, "Time24"))
            {
                context.AddQuery(new ClientActionSetProperty(settings, "Time24", definition.Time24));
                shouldUpdate = true;
            }
        }

        #endregion
    }
}
