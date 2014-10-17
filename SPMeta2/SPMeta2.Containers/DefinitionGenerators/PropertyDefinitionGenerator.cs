using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class PropertyDefinitionGenerator : TypedDefinitionGeneratorServiceBase<PropertyDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Key = Rnd.String();
                def.Value = Rnd.String();

                def.Overwrite = Rnd.Bool();
            });
        }
    }
}
