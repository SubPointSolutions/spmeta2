using System;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class TaxonomyFieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddTaxonomyField(this ModelNode model, TaxonomyFieldDefinition definition)
        {
            return AddTaxonomyField(model, definition, null);
        }

        public static ModelNode AddTaxonomyField(this ModelNode model, TaxonomyFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
