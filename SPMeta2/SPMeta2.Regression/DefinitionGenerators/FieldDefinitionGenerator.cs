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
    public class SecurityGroupDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SecurityGroupDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.Description = Rnd.String();

                def.Owner = Rnd.UserLogin();
                def.DefaultUser = Rnd.UserLogin();
            });
        }
    }
}
