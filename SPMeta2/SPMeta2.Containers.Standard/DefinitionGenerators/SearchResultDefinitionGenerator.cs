using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class SearchResultDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SearchResultDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.IsDefault = Rnd.Bool();

                def.ProviderName = "Local SharePoint Provider";
                def.Query = string.Format("{{?{{searchTerms}} -ContentClass=urn:content-class:SPSPeople AND Title:\"{0}\"}}", Rnd.String());
            });
        }
    }
}
