using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class AppPrincipalDefinitionGenerator : TypedDefinitionGeneratorServiceBase<AppPrincipalDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                var domain = Rnd.String(32);

                def.AppId = Rnd.String(32);
                def.AppSecret = Rnd.String(32);

                def.Title = Rnd.String(32);

                def.AppDomain = string.Format("www.{0}.com", domain);
                def.RedirectURI = string.Format("https://{0}.com/app", domain);
            });
        }
    }
}
