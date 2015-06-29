using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class TaxonomyTermGroupModelNode : TypedModelNode
    {

    }

    public static class TaxonomyTermGroupDefinitionSyntax
    {
        public static TaxonomyTermStoreModelNode AddTaxonomyTermGroup(this TaxonomyTermStoreModelNode model,
            TaxonomyTermGroupDefinition definition)
        {
            return AddTaxonomyTermGroup(model, definition, null);
        }

        public static TaxonomyTermStoreModelNode AddTaxonomyTermGroup(this TaxonomyTermStoreModelNode model,
            TaxonomyTermGroupDefinition definition, Action<TaxonomyTermGroupModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #region array overload

        public static ModelNode AddTaxonomyTermGroups(this ModelNode model, IEnumerable<TaxonomyTermGroupDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static ModelNode AddHostTaxonomyTermGroup(this ModelNode model, 
            TaxonomyTermGroupDefinition definition)
        {
            return AddHostTaxonomyTermGroup(model, definition, null);
        }

        public static ModelNode AddHostTaxonomyTermGroup(this ModelNode model, TaxonomyTermGroupDefinition definition,
            Action<TaxonomyTermGroupModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
