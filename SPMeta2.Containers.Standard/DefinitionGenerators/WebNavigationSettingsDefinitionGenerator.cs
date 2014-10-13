using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class WebNavigationSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WebNavigationSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.CurrentNavigationMaximumNumberOfDynamicItems = Rnd.Int(15) + 1;
                def.CurrentNavigationShowPages = Rnd.Bool();
                def.CurrentNavigationShowSubsites = Rnd.Bool();

                def.GlobalNavigationMaximumNumberOfDynamicItems = Rnd.Int(15) + 1;
                def.GlobalNavigationShowPages = Rnd.Bool();
                def.GlobalNavigationShowSubsites = Rnd.Bool();

                // TODO, source
            });
        }
    }
}
