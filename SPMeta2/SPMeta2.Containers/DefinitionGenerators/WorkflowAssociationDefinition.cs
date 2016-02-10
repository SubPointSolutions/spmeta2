using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class WorkflowAssociationDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WorkflowAssociationDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.Description = Rnd.String();

                def.WorkflowTemplateName = "Approval - SharePoint 2010";

                def.TaskListTitle = "Workflow Tasks";
                def.HistoryListTitle = "Workflow History";
            });
        }
    }
}
