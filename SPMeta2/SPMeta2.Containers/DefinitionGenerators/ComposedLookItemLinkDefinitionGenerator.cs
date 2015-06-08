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
    public class ComposedLookItemLinkDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ComposedLookItemLinkDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.ComposedLookItemName = Rnd.RandomFromArray(new[]{
                    BuiltInComposedLookItemNames.Breeze,
                    BuiltInComposedLookItemNames.Blossom,
                    BuiltInComposedLookItemNames.City,
                    BuiltInComposedLookItemNames.Green,
                    BuiltInComposedLookItemNames.Red,
                    BuiltInComposedLookItemNames.Office,
               });
            });
        }
    }
}
