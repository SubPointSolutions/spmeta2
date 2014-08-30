using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.DefinitionGenerators
{
    public class JobDefinitionGenerator : TypedDefinitionGeneratorServiceBase<JobDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();

                // TODO, we need a helper here
                //http://blogs.msdn.com/b/markarend/archive/2007/01/16/spschedule-fromstring-recurrencevalue-syntax-format-for-recurrence-value.aspx
                def.ScheduleString = "yearly at jan 1 09:00:00";

                // TMP
                def.JobType = "Microsoft.SharePoint.Administration.SPUpgradeJobDefinition, Microsoft.SharePoint";
            });
        }
    }
}
