using System;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Fields;

namespace SPMeta2.Containers.Standard.DefinitionGenerators.Fields
{
    public class ImageFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(ImageFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            var def = new ImageFieldDefinition
            {

            };


            return def;
        }
    }
}
