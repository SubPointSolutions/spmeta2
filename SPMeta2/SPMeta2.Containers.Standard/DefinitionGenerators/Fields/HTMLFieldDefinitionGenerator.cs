using System;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Fields;

namespace SPMeta2.Containers.Standard.DefinitionGenerators.Fields
{
    public class TextFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(HTMLFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            var def = new HTMLFieldDefinition
            {

            };


            return def;
        }
    }
}
