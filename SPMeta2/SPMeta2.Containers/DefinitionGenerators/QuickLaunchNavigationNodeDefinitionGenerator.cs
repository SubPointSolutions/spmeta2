using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class QuickLaunchNavigationNodeDefinitionGenerator : TypedDefinitionGeneratorServiceBase<QuickLaunchNavigationNodeDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.Url = Rnd.String();

                def.IsVisible = Rnd.Bool();
                def.IsExternal = true;
            });
        }
    }
}
