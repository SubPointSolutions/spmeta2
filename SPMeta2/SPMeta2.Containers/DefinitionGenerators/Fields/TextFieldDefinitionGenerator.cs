using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class TextFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(TextFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            var def = new TextFieldDefinition
            {
                MaxLength = Rnd.Int(32) + 1
            };

            return def;
        }

    }
}
