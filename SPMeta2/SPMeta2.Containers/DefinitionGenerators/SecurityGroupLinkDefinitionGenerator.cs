using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class SecurityGroupLinkDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SecurityGroupLinkDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                // TODO
                // name resolution does not work well with localization
                def.SecurityGroupName = "Approvers";

                //def.IsAssociatedMemberGroup = true;
            });
        }
    }
}
