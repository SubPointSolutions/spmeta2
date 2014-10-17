using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class WebApplicationDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WebApplicationDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Port = 1000 + Rnd.Int(20000);

                def.AllowAnonymousAccess = false;
                def.ManagedAccount = Rnd.UserName();

                def.CreateNewDatabase = true;

                def.DatabaseServer = Rnd.DbServerName();
                def.DatabaseName = Rnd.String();
            });
        }
    }
}
