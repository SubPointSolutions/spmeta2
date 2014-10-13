using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class TaxonomyTermSetDefinitionSyntax
    {
        public static ModelNode AddTermSet(this ModelNode model, TaxonomyTermSetDefinition definition)
        {
            return AddTermSet(model, definition, null);
        }

        public static ModelNode AddTermSet(this ModelNode model, TaxonomyTermSetDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }
    }
}
