using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class SP2013WorkflowDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SP2013WorkflowDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.DisplayName = Rnd.String();
                def.Override = Rnd.Bool();

                def.Xaml = WorkflowXAMLDefinitions.WriteToHistoryListWorkflow;
            });
        }
    }
}
