using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class TaxonomyTermDefinitionSyntax
    {
        public static ModelNode AddTaxonomyTerm(this ModelNode model, TaxonomyTermDefinition definition)
        {
            return AddTaxonomyTerm(model, definition, null);
        }

        public static ModelNode AddTaxonomyTerm(this ModelNode model, TaxonomyTermDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #region array overload

        public static ModelNode AddTaxonomyTerms(this ModelNode model, IEnumerable<TaxonomyTermDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static ModelNode AddHostTaxonomyTerm(this ModelNode model, TaxonomyTermDefinition definition)
        {
            return AddHostTaxonomyTerm(model, definition, null);
        }

        public static ModelNode AddHostTaxonomyTerm(this ModelNode model, TaxonomyTermDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
