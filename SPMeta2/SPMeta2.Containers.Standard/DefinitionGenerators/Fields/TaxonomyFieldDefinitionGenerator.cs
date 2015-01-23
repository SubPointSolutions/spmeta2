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
                DefaultValue = string.Empty
            };
        }

        protected override void PostProcessDefinitionTemplate(FieldDefinition action)
        {
            var def = action as TaxonomyFieldDefinition;
            def.DefaultValue = string.Empty;
        }
    }
}
