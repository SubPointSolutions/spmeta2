using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class ChoiceFieldFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(ChoiceFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            var def = new ChoiceFieldDefinition();

            var choiceCount = Rnd.Int(10) + 1;

            for (var index = 0; index < choiceCount; index++)
            {
                def.Choices.Add(Rnd.String(8));
            }

            def.ValidationMessage = string.Format("validatin_msg_{0}", Rnd.String());
            def.ValidationFormula = string.Format("=[ID] * {0}", Rnd.Int(100));

            return def;
        }

    }
}
