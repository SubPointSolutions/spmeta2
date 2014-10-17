using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators
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
