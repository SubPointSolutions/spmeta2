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
    public class SecurityGroupLinkDefinitionGenerator : TypedDefinitionGeneratorServiceBase<SecurityGroupLinkDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                // TODO
                // we need to resolve 'parent' TMP security group and link it here
                def.SecurityGroupName = "Approvers";
            });
        }
    }
}
