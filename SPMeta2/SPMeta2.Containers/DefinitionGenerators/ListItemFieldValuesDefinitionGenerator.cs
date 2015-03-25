using System;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ListItemFieldValuesDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ListItemFieldValuesDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                // FieldId is not supported by CSOM, so we use field name
                // http://officespdev.uservoice.com/forums/224641-general/suggestions/6411772-expose-guid-field-value-indexer-for-microsoft-shar

                def.Values.Add(new FieldValue()
                {
                    FieldName = "Order",
                    Value = Rnd.Int(32)
                });
            });
        }
    }
}
