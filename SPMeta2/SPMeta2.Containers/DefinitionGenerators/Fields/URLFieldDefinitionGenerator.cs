using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class URLFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(URLFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            return new URLFieldDefinition
            {
                DisplayFormat = Rnd.Bool() ?
                    BuiltInUrlFieldFormatType.Hyperlink :
                    BuiltInUrlFieldFormatType.Image
            };
        }
    }
}
