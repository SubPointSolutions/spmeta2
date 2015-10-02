using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class SummaryLinkFieldModelNode : FieldModelNode
    {

    }

    public static class SummaryLinkFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSummaryLinkField<TModelNode>(this TModelNode model, SummaryLinkFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddSummaryLinkField(model, definition, null);
        }

        public static TModelNode AddSummaryLinkField<TModelNode>(this TModelNode model, SummaryLinkFieldDefinition definition,
            Action<SummaryLinkFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSummaryLinkFields<TModelNode>(this TModelNode model, IEnumerable<SummaryLinkFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
