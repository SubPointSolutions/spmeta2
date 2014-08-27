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
    public class SecurityRoleDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SecurityRoleDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.BasePermissions = new System.Collections.ObjectModel.Collection<string>
                {
                    "AddListItems",
                    "EditListItems",
                    "OpenItems",
                    "ManageLists"
                };
                def.Description = Rnd.String();
            });
        }
    }
}
