using System;
using System.Collections.ObjectModel;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class JobDefinitionGenerator : TypedDefinitionGeneratorServiceBase<JobDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.Title = Rnd.String();

                // TODO, we need a helper here
                //http://blogs.msdn.com/b/markarend/archive/2007/01/16/spschedule-fromstring-recurrencevalue-syntax-format-for-recurrence-value.aspx
                def.ScheduleString = "yearly at jan 1 09:00:00";

                // TMP
                def.JobType = "Microsoft.SharePoint.Administration.SPDeadSiteDeleteJobDefinition, Microsoft.SharePoint";

                def.ConstructorParams = new Collection<JobDefinitionCtorParams>()
                {
                    JobDefinitionCtorParams.JobName,
                    JobDefinitionCtorParams.WebApplication
                };
            });
        }
    }
}
