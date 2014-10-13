using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class TaxonomyTermDefinitionSyntax
    {
        public static ModelNode AddTerm(this ModelNode model, TaxonomyTermDefinition definition)
        {
            return AddTerm(model, definition, null);
        }

        public static ModelNode AddTerm(this ModelNode model, TaxonomyTermDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }
    }
}
