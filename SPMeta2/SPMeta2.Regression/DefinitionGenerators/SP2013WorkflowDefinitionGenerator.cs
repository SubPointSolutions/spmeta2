using SPMeta2.Definitions;
using SPMeta2.Regression.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.DefinitionGenerators
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
