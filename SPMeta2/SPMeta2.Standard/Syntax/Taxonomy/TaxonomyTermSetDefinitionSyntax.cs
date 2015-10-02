using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class TaxonomyTermSetModelNode : TypedModelNode,
        ITaxonomyTermHostModelNode
    {

    }

    public static class TaxonomyTermSetDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTaxonomyTermSet<TModelNode>(this TModelNode model, TaxonomyTermSetDefinition definition)
            where TModelNode : ModelNode, ITaxonomyTermSetHostModelNode, new()
        {
            return AddTaxonomyTermSet(model, definition, null);
        }

        public static TModelNode AddTaxonomyTermSet<TModelNode>(this TModelNode model, TaxonomyTermSetDefinition definition,
            Action<TaxonomyTermSetModelNode> action)
            where TModelNode : ModelNode, ITaxonomyTermSetHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddTaxonomyTermSets<TModelNode>(this TModelNode model, IEnumerable<TaxonomyTermSetDefinition> definitions)
           where TModelNode : ModelNode, ITaxonomyTermSetHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion


        public static TModelNode AddHostTaxonomyTermSet<TModelNode>(this TModelNode model, TaxonomyTermSetDefinition definition)
             where TModelNode : ModelNode, ITaxonomyTermSetHostModelNode, new()
        {
            return AddHostTaxonomyTermSet(model, definition, null);
        }
        public static TModelNode AddHostTaxonomyTermSet<TModelNode>(this TModelNode model, TaxonomyTermSetDefinition definition,
            Action<TaxonomyTermSetModelNode> action)
            where TModelNode : ModelNode, ITaxonomyTermSetHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
