using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class NumberFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(NumberFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            return new NumberFieldDefinition
            {
                MinimumValue = Rnd.Int(100),
                MaximumValue = Rnd.Int(100) + 1000,
                ShowAsPercentage = Rnd.Bool(),
                DisplayFormat = BuiltInNumberFormatTypes.ThreeDecimals,
                ValidationMessage = string.Format("validatin_msg_{0}", Rnd.String()),
                ValidationFormula = string.Format("=[ID] * {0}", Rnd.Int(100))
            };
        }
    }
}
