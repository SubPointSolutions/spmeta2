using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class BooleanFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(BooleanFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            return new BooleanFieldDefinition
            {

            };
        }

        protected override void PostProcessDefinitionTemplate(FieldDefinition def)
        {
            base.PostProcessDefinitionTemplate(def);

            (def as BooleanFieldDefinition).DefaultValue = Rnd.Bool() ? "0" : "1";
        }
    }
}
