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
    public class LinkFieldModelNode : FieldModelNode
    {

    }

    public static class LinkFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddLinkField<TModelNode>(this TModelNode model, LinkFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddLinkField(model, definition, null);
        }

        public static TModelNode AddLinkField<TModelNode>(this TModelNode model, LinkFieldDefinition definition,
            Action<LinkFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddLinkFields<TModelNode>(this TModelNode model, IEnumerable<LinkFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
