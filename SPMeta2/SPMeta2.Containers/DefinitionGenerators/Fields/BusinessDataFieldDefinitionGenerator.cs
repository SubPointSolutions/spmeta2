using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class BusinessDataFieldDefinitionDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(BusinessDataFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            return new BusinessDataFieldDefinition
            {
                EntityName = Rnd.String(),
                EntityNamespace = Rnd.String(),
                BdcFieldName = Rnd.String(),
                SystemInstanceName = Rnd.String()
            };
        }
    }
}
