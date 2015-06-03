using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Containers.Services.Base;

namespace SPMeta2.Containers.Standard.DefinitionGenerators.Webparts
{
    public class RefinementScriptWebPartDefinitionGenerator : TypedDefinitionGeneratorServiceBase<RefinementScriptWebPartDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.String();
                def.Title = Rnd.String();

                def.ZoneId = "FullPage";
                def.ZoneIndex = Rnd.Int(100);
            });
        }
    }
}
