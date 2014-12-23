using System;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class WebConfigModificationDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WebConfigModificationDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Path = string.Format("configuration/system.web/pages");
                def.Name = string.Format("attr{0}", Rnd.String(4));
                def.Sequence = Rnd.UInt(100);
                def.Owner = string.Format("WebConfigModifications{0}", Rnd.String(8));
                def.Type = BuiltInWebConfigModificationType.EnsureAttribute;
                def.Value = Rnd.Bool().ToString().ToLower();
            });
        }
    }
}
