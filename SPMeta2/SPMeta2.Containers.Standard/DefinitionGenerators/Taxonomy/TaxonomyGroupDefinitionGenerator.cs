using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;

namespace SPMeta2.Containers.Standard.DefinitionGenerators.Taxonomy
{
    public class TaxonomyGroupDefinitionGenerator : TypedDefinitionGeneratorServiceBase<TaxonomyTermGroupDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.Id = null;
            });
        }
    }
}
