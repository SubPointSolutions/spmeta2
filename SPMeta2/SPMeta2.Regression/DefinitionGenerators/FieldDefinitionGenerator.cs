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
    public class FieldDefinitionGenerator : TypedDefinitionGeneratorServiceBase<FieldDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.Guid();
                def.InternalName = Rnd.String(32);

                def.Description = Rnd.String();
                def.FieldType = BuiltInFieldTypes.Text;

                def.Required = Rnd.Bool();

                def.Group = Rnd.String();
                def.Title = Rnd.String(32);

            });
        }
    }
}
