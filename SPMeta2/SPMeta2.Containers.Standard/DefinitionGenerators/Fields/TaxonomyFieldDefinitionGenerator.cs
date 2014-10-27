using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Fields;

namespace SPMeta2.Containers.Standard.DefinitionGenerators.Fields
{
    public class TaxonomyFieldDefinitionGenerator : TypedDefinitionGeneratorServiceBase<TaxonomyFieldDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.Guid();
                def.InternalName = Rnd.String(32);

                def.Description = Rnd.String();

                def.Required = Rnd.Bool();

                def.Group = Rnd.String();
                def.Title = Rnd.String(32);

                def.UseDefaultSiteCollectionTermStore = true;
                //def.SspName = "Managed Metadata Application";
                def.TermSetName = "Maps";
            });
        }
    }
}
