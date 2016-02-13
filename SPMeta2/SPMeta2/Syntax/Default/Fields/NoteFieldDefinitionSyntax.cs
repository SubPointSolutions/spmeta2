using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class NoteFieldModelNode : FieldModelNode
    {

    }

    public static class NoteFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddNoteField<TModelNode>(this TModelNode model, NoteFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddNoteField(model, definition, null);
        }

        public static TModelNode AddNoteField<TModelNode>(this TModelNode model, NoteFieldDefinition definition,
            Action<NoteFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddNoteFields<TModelNode>(this TModelNode model, IEnumerable<NoteFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
