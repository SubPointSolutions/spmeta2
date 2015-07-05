using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class TaxonomyFieldModelNode : FieldModelNode
    {

    }

    public static class TaxonomyFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTaxonomyField<TModelNode>(this TModelNode model, TaxonomyFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddTaxonomyField(model, definition, null);
        }

        public static TModelNode AddTaxonomyField<TModelNode>(this TModelNode model, TaxonomyFieldDefinition definition,
            Action<TaxonomyFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddTaxonomyFields<TModelNode>(this TModelNode model, IEnumerable<TaxonomyFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
