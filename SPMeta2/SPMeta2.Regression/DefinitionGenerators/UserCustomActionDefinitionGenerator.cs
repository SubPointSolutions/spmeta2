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
    public class UserCustomActionDefinitionGenerator : TypedDefinitionGeneratorServiceBase<UserCustomActionDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.Location = "ScriptLink";
                def.ScriptSrc = "~site/style library/spmeta2.js";
                def.Sequence = Rnd.Int(100);
                def.Description = Rnd.String();
                def.Group = Rnd.String();
            });
        }
    }
}
