using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Definitions.Taxonomy;

namespace SPMeta2.Containers.DefinitionGenerators.Taxonomy
{
    public class TaxonomyTermDefinitionGenerator : TypedDefinitionGeneratorServiceBase<TaxonomyTermDefinition>
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
