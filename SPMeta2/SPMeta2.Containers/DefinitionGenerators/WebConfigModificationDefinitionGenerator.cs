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
                def.Path = string.Format("configuration/appSettings");
                def.Name = string.Format("add [@key='k{0}'] [@value='{1}']", Rnd.String(8), Rnd.String(8));
                def.Sequence = Rnd.UInt(100);
                def.Owner = string.Format("WebConfigModifications{0}", Rnd.String(8));
                def.Type = BuiltInWebConfigModificationType.EnsureChildNode;
                def.Value = Rnd.Bool().ToString().ToLower();
            });
        }
    }
}
