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
                // name resolution does not work well with localization
                //def.SecurityGroupName = "Approvers";

                def.IsAssociatedMemberGroup = true;
            });
        }
    }
}
