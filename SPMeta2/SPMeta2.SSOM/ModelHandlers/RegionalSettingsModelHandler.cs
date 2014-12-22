using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

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
            settings.AdjustHijriDays = definition.AdjustHijriDays;
            settings.AlternateCalendarType = definition.AlternateCalendarType;
            settings.CalendarType = definition.CalendarType;
            settings.Collation = definition.Collation;
            settings.FirstDayOfWeek = definition.FirstDayOfWeek;
            settings.FirstWeekOfYear = definition.FirstWeekOfYear;
            settings.LocaleId = definition.LocaleId;
            settings.WorkDayStartHour = definition.WorkDayStartHour;
            settings.WorkDayEndHour = definition.WorkDayEndHour;
            settings.WorkDays = definition.WorkDays;
            settings.ShowWeeks = definition.ShowWeeks;
            settings.Time24 = definition.Time24;
            settings.LocaleId = definition.LocaleId;
        }

        #endregion
    }
}
