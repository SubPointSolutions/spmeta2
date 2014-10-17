using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class UserCustomActionDefinitionGenerator : TypedDefinitionGeneratorServiceBase<UserCustomActionDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.Location = "ScriptLink";
                def.ScriptSrc = "~site/style library/spmeta2.js";
                def.Sequence = Rnd.Int(100);
                def.Description = Rnd.String();
                def.Group = Rnd.String();
            });
        }
    }
}
