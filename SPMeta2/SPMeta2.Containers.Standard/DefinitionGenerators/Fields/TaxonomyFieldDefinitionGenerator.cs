using System;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Fields;

namespace SPMeta2.Containers.Standard.DefinitionGenerators.Fields
{
    public class TaxonomyFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(TaxonomyFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            return new TaxonomyFieldDefinition
            {
                UseDefaultSiteCollectionTermStore = true,
                TermSetName = "Maps"
            };
        }
    }
}
