using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class CurrencyFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(CurrencyFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            return new CurrencyFieldDefinition
            {
                CurrencyLocaleId = 1040 + Rnd.Int(5),
                ValidationMessage = string.Format("validatin_msg_{0}", Rnd.String()),
                ValidationFormula = string.Format("=[ID] * {0}", Rnd.Int(100))
            };
        }
    }
}
