using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class MultiChoiceFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(MultiChoiceFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            var def = new MultiChoiceFieldDefinition();

            var choiceCount = Rnd.Int(10) + 1;

            for (var index = 0; index < choiceCount; index++)
            {
                def.Choices.Add(Rnd.String(8));
                def.Mappings.Add(Rnd.String(8));
            }

            return def;
        }
    }
}
