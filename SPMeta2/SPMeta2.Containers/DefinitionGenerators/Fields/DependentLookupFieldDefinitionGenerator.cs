using System;
using System.Collections.Generic;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{

    public class DependentLookupFieldDefinitionGenerator : TypedDefinitionGeneratorServiceBase<DependentLookupFieldDefinition>
    {
        protected LookupFieldDefinition PrimaryLookupField =
            new LookupFieldDefinitionGenerator().GenerateRandomDefinition() as LookupFieldDefinition;

        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String(12);
                def.InternalName = string.Format("iname_{0}", Rnd.String(12));

                def.PrimaryLookupFieldId = PrimaryLookupField.Id;
            });
        }

        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            return new[] { PrimaryLookupField };
        }
    }
}
