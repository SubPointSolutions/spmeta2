using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions.Taxonomy;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class TaxonomyTermStoreDefinitionSyntax
    {
        public static ModelNode AddTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition)
        {
            return AddTermStore(model, definition, null);
        }

        public static ModelNode AddTermStore(this ModelNode model, TaxonomyTermStoreDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }
    }
}
