using System;
using System.Collections.Generic;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class TaxonomyTermSetModelNode : TypedModelNode
    {

    }

    public static class TaxonomyTermSetDefinitionSyntax
    {
        public static TaxonomyTermGroupModelNode AddTaxonomyTermSet(this TaxonomyTermGroupModelNode model, TaxonomyTermSetDefinition definition)
        {
            return AddTaxonomyTermSet(model, definition, null);
        }

        public static TaxonomyTermGroupModelNode AddTaxonomyTermSet(this TaxonomyTermGroupModelNode model,
            TaxonomyTermSetDefinition definition, Action<TaxonomyTermSetModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #region array overload

        public static ModelNode AddTaxonomyTermSets(this ModelNode model, IEnumerable<TaxonomyTermSetDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static TaxonomyTermGroupModelNode AddHostTaxonomyTermSet(this TaxonomyTermGroupModelNode model,
            TaxonomyTermSetDefinition definition)
        {
            return AddHostTaxonomyTermSet(model, definition, null);
        }

        public static TaxonomyTermGroupModelNode AddHostTaxonomyTermSet(this TaxonomyTermGroupModelNode model, TaxonomyTermSetDefinition definition,
            Action<TaxonomyTermSetModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
