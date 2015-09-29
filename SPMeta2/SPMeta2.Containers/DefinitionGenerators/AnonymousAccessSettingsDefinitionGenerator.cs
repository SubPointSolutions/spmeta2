using System;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class AnonymousAccessSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<AnonymousAccessSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.AnonymousState = Rnd.RandomFromArray(new[]
                {
                    BuiltInWebAnonymousState.Disabled,
                    BuiltInWebAnonymousState.Enabled,
                });

                if (def.AnonymousState == BuiltInWebAnonymousState.On)
                {
                    def.AnonymousPermMask64 = new System.Collections.ObjectModel.Collection<string>
                    {
                        "Open"
                    };
                }
                if (def.AnonymousState == BuiltInWebAnonymousState.Enabled)
                {
                    def.AnonymousPermMask64 = new System.Collections.ObjectModel.Collection<string>
                    {
                        "AddListItems",
                        "EditListItems",
                        "OpenItems",
                        "ManageLists"
                    };
                }
            });
        }
    }
}
