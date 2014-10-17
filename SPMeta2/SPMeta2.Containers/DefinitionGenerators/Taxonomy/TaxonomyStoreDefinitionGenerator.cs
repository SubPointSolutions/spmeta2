using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;

namespace SPMeta2.Containers.DefinitionGenerators.Taxonomy
{
    public class TaxonomyStoreDefinitionGenerator : TypedDefinitionGeneratorServiceBase<TaxonomyTermStoreDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                // hardcoded yet

                //def.Name = "Managed Metadata Application";
                //def.Id = null;

                //def.UseDefaultSiteCollectionTermStore = true;
                def.Name = "Managed Metadata Application";
            });
        }
    }
}
