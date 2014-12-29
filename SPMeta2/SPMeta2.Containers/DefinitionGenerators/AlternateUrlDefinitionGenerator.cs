using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class AlternateUrlDefinitionGenerator : TypedDefinitionGeneratorServiceBase<AlternateUrlDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Url = Rnd.HttpUrl();

                switch (Rnd.Int(4))
                {
                    case 0:
                        def.UrlZone = BuiltInUrlZone.Custom;
                        break;

                    case 1:
                        def.UrlZone = BuiltInUrlZone.Extranet;
                        break;

                    case 2:
                        def.UrlZone = BuiltInUrlZone.Internet;
                        break;

                    case 3:
                        def.UrlZone = BuiltInUrlZone.Intranet;
                        break;

                    default:
                        def.UrlZone = BuiltInUrlZone.Extranet;
                        break;
                }
            });
        }
    }
}
