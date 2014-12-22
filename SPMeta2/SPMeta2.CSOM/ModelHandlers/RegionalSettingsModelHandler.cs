using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
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

            MapRegionalSettings(settings, definition);

            //web.RegionalSettings = settings;

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
        }

        protected RegionalSettings GetCurrentRegionalSettings(Web web)
        {
            return web.RegionalSettings;
        }

        private void MapRegionalSettings(RegionalSettings settings, RegionalSettingsDefinition definition)
        {
            // TODO, CSOM does not support any operation for RegionalSetting yet

            //settings.AdjustHijriDays = definition.AdjustHijriDays;
            //settings.AlternateCalendarType = definition.AlternateCalendarType;
            //settings.CalendarType = definition.CalendarType;
            //settings.Collation = definition.Collation;
            //settings.FirstDayOfWeek = definition.FirstDayOfWeek;
            //settings.FirstWeekOfYear = definition.FirstWeekOfYear;
            //settings.LocaleId = definition.LocaleId;
            //settings.WorkDayStartHour = definition.WorkDayStartHour;
            //settings.WorkDayEndHour = definition.WorkDayEndHour;
            //settings.WorkDays = definition.WorkDays;
            //settings.ShowWeeks = definition.ShowWeeks;
            //settings.Time24 = definition.Time24;
            //settings.LocaleId = definition.LocaleId;
        }

        #endregion
    }
}
