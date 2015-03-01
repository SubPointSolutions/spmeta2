using System;
using System.Collections.ObjectModel;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers.DefinitionGenerators.Fields
{
    public class CalculatedFieldDefinitionGenerator : FieldDefinitionGenerator
    {
        public override Type TargetType
        {
            get { return typeof(CalculatedFieldDefinition); }
        }

        protected override FieldDefinition GetFieldDefinitionTemplate()
        {
            return new CalculatedFieldDefinition
            {
                CurrencyLocaleId = 1040 + Rnd.Int(5),
                DateFormat = BuiltInDateTimeFieldFormatType.DateTime,
                DisplayFormat = BuiltInNumberFormatTypes.FourDecimals,
                FieldReferences = new Collection<string>  {
                    BuiltInInternalFieldNames.ID,
                   BuiltInInternalFieldNames.FileType
                },
                Formula = string.Format("=ID*{0}", Rnd.Int(100)),
                OutputType = BuiltInFieldTypes.Number
            };
        }
    }
}
