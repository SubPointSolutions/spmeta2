using System;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class AssociatedGroupsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<AssociatedGroupsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                var groupNames = new[]
                {
                    "SPMeta2 Test Group 1",
                    "SPMeta2 Test Group 2",
                    "SPMeta2 Test Group 3",
                    "SPMeta2 Test Group 4",
                    "SPMeta2 Test Group 5"
                };

                def.MemberGroupName = Rnd.RandomFromArray(groupNames);
                def.OwnerGroupName = Rnd.RandomFromArray(groupNames);
                def.VisitorGroupName = Rnd.RandomFromArray(groupNames);
            });
        }
    }
}
