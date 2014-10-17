using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class PrefixDefinitionGenerator : TypedDefinitionGeneratorServiceBase<PrefixDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Path = Rnd.String();
                def.PrefixType = BuiltInPrefixTypes.WildcardInclusion;
            });
        }
    }
}
