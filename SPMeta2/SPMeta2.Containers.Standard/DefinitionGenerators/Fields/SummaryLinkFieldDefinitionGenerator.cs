using System;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Fields;

namespace SPMeta2.Containers.Standard.DefinitionGenerators.Fields
{
    public class SummaryLinkFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(SummaryLinkFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            var def = new SummaryLinkFieldDefinition
            {

            };


            return def;
        }
    }
}
