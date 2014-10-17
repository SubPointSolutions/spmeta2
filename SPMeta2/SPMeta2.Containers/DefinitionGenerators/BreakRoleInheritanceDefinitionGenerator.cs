using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class BreakRoleInheritanceDefinitionGenerator : TypedDefinitionGeneratorServiceBase<BreakRoleInheritanceDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.CopyRoleAssignments = Rnd.Bool();
                def.ClearSubscopes = Rnd.Bool();
            });
        }
    }
}
