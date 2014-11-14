using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class SecurityGroupDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SecurityGroupDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.Description = Rnd.String();

                def.OnlyAllowMembersViewMembership = Rnd.Bool();

                def.Owner = Rnd.UserLogin();
                def.DefaultUser = Rnd.UserLogin();
            });
        }
    }
}
