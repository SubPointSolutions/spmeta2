using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class TaxonomyTermGroupDefinitionSyntax
    {
        public static ModelNode AddTermGroup(this ModelNode model, TaxonomyTermGroupDefinition definition)
        {
            return AddTermGroup(model, definition, null);
        }

        public static ModelNode AddTermGroup(this ModelNode model, TaxonomyTermGroupDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }
    }
}
