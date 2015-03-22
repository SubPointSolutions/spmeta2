using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class GeolocationFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(GeolocationFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            return new GeolocationFieldDefinition
            {

            };
        }
    }
}
