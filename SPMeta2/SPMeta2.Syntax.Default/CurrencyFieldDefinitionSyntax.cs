using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class CurrencyFieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddCurrencyField(this ModelNode model, CurrencyFieldDefinition definition)
        {
            return AddCurrencyField(model, definition, null);
        }

        public static ModelNode AddCurrencyField(this ModelNode model, CurrencyFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddCurrencyFields(this ModelNode model, IEnumerable<CurrencyFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
