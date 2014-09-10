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
    public class SP2013WorkflowSubscriptionDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SP2013WorkflowSubscriptionDefinition>
    {
        #region constructors

        public SP2013WorkflowSubscriptionDefinitionGenerator()
        {
            WorkflowName = Rnd.String();
        }

        #endregion

        #region properties

        public string WorkflowName { get; set; }

        #endregion

        #region methods

        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.WorkflowDisplayName = WorkflowName;

                def.EventTypes = new System.Collections.ObjectModel.Collection<string>
                {
                    BuiltInSP2013WorkflowEventTypes.ItemAdded
                };

                // a big TODO as these list don't exist yet

                def.HistoryListUrl = "/WorkflowTasks";
                def.TaskListUrl = "/Lists/List";
            });
        }

        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            var workflowDefinitionGenerator = new SP2013WorkflowDefinitionGenerator();
            var workflowDefinition = workflowDefinitionGenerator.GenerateRandomDefinition() as SP2013WorkflowDefinition;

            workflowDefinition.DisplayName = WorkflowName;

            return new[] { workflowDefinition };
        }

        #endregion
    }
}
