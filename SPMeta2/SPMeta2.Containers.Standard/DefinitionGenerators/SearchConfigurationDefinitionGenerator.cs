using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class SearchConfigurationDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SearchConfigurationDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.SearchConfiguration = SearchTemplates.DefaultSearchConfiguration;
            });
        }
    }
}
