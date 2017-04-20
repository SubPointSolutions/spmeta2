using System;
using System.Collections.ObjectModel;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class SuiteBarDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SuiteBarDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.SuiteBarBrandingElementHtml = Rnd.String();
            });
        }
    }
}
