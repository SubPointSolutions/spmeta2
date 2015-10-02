using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class TaxonomyTermGroupModelNode : TypedModelNode,
        ITaxonomyTermSetHostModelNode
    {

    }

    public static class TaxonomyTermGroupDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTaxonomyTermGroup<TModelNode>(this TModelNode model, TaxonomyTermGroupDefinition definition)
            where TModelNode : ModelNode, ITaxonomyTermGroupHostModelNode, new()
        {
            return AddTaxonomyTermGroup(model, definition, null);
        }

        public static TModelNode AddTaxonomyTermGroup<TModelNode>(this TModelNode model, TaxonomyTermGroupDefinition definition,
            Action<TaxonomyTermGroupModelNode> action)
            where TModelNode : ModelNode, ITaxonomyTermGroupHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddTaxonomyTermGroups<TModelNode>(this TModelNode model, IEnumerable<TaxonomyTermGroupDefinition> definitions)
           where TModelNode : ModelNode, ITaxonomyTermGroupHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static TModelNode AddHostTaxonomyTermGroup<TModelNode>(this TModelNode model, TaxonomyTermGroupDefinition definition)
            where TModelNode : ModelNode, ITaxonomyTermGroupHostModelNode, new()
        {
            return AddHostTaxonomyTermGroup(model, definition, null);
        }
        public static TModelNode AddHostTaxonomyTermGroup<TModelNode>(this TModelNode model, TaxonomyTermGroupDefinition definition,
            Action<TaxonomyTermGroupModelNode> action)
            where TModelNode : ModelNode, ITaxonomyTermGroupHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
