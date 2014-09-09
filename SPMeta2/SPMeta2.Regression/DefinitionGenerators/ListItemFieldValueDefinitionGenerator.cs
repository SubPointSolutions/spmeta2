using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.DefinitionGenerators
{
    public class ListItemFieldValueDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ListItemFieldValueDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                //def.FieldId = BuiltInFieldId.Order;

                // FieldId is not supported by CSOM, so we use field name
                // http://officespdev.uservoice.com/forums/224641-general/suggestions/6411772-expose-guid-field-value-indexer-for-microsoft-shar

                def.FieldName = "Order";
                def.Value = Math.Ceiling(Rnd.Double(100));
            });
        }
    }
}
