using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.DefinitionGenerators
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
