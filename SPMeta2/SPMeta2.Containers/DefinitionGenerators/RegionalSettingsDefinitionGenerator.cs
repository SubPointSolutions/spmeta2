using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class RegionalSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<RegionalSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.AdjustHijriDays = Rnd.Short(5);
                def.AlternateCalendarType = Rnd.Short(5);
                def.CalendarType = Rnd.Short(5);
                def.Collation = Rnd.Short(5);
                def.FirstDayOfWeek = Rnd.UInt(7);
                def.FirstWeekOfYear = Rnd.Short(2);
                def.LocaleId = (uint)(1040 + Rnd.Int(5));
                def.WorkDayStartHour = Rnd.Short(14);
                def.WorkDayEndHour = (short)(def.WorkDayStartHour + 5);
                def.WorkDays = Rnd.Short(5);
                def.ShowWeeks = Rnd.Bool();
                def.Time24 = Rnd.Bool();
            });
        }
    }
}
