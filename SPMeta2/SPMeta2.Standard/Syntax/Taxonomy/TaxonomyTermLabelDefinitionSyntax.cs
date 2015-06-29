using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Standard.Syntax
{
    public static class TaxonomyTermLabelDefinitionSyntax
    {
        public static TaxonomyTermModelNode AddTaxonomyTermLabel(this TaxonomyTermModelNode model, TaxonomyTermLabelDefinition definition)
        {
            return AddTaxonomyTermLabel(model, definition, null);
        }

        public static TaxonomyTermModelNode AddTaxonomyTermLabel(this TaxonomyTermModelNode model, TaxonomyTermLabelDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #region array overload

        public static TaxonomyTermModelNode AddTaxonomyTermLabels(this TaxonomyTermModelNode model, IEnumerable<TaxonomyTermLabelDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
