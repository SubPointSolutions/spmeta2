using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class SupportedUICultureDefinitionGenerator :
        TypedDefinitionGeneratorServiceBase<SupportedUICultureDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                // TODO, setup eandim out of the avialable test language set
                def.LCID = 1033;
            });
        }
    }
}
