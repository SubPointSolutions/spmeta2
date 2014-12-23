using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class InformationRightsManagementSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<InformationRightsManagementSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                // TODO
            });
        }
    }
}
