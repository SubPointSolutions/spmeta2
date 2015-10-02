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
    public class HTMLFieldModelNode : FieldModelNode
    {

    }

    public static class HTMLFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddHTMLField<TModelNode>(this TModelNode model, HTMLFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddHTMLField(model, definition, null);
        }

        public static TModelNode AddHTMLField<TModelNode>(this TModelNode model, HTMLFieldDefinition definition,
            Action<HTMLFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddHTMLFields<TModelNode>(this TModelNode model, IEnumerable<HTMLFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
