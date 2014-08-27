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
    public class PropertyDefinitionGenerator : TypedDefinitionGeneratorServiceBase<PropertyDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Key = Rnd.String();
                def.Value = Rnd.String();

                def.Overwrite = Rnd.Bool();
            });
        }
    }
}
