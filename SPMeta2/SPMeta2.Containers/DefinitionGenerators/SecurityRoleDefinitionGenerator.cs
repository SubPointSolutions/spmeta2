using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class SecurityRoleDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SecurityRoleDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.BasePermissions = new System.Collections.ObjectModel.Collection<string>
                {
                    "AddListItems",
                    "EditListItems",
                    "OpenItems",
                    "ManageLists"
                };
                def.Description = Rnd.String();
            });
        }
    }
}
