using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class MultiChoiceFieldDefinitionGenerator : TypedDefinitionGeneratorServiceBase<MultiChoiceFieldDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Id = Rnd.Guid();
                def.InternalName = Rnd.String(32);

                def.Description = Rnd.String();
                //def.FieldType = BuiltInFieldTypes.Text;

                def.Required = Rnd.Bool();

                def.Group = Rnd.String();
                def.Title = Rnd.String(32);

                var choiceCount = Rnd.Int(10) + 1;

                for (var index = 0; index < choiceCount; index++)
                {
                    def.Choices.Add(Rnd.String(8));
                }

                // TODO
            });
        }
    }
}
