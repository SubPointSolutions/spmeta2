using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class DocumentSetModelNode : TypedModelNode
    {

    }

    public static class DocumentSetDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDocumentSet<TModelNode>(this TModelNode model, DocumentSetDefinition definition)
            where TModelNode : ModelNode, IFolderHostModelNode, new()
        {
            return AddDocumentSet(model, definition, null);
        }

        public static TModelNode AddDocumentSet<TModelNode>(this TModelNode model, DocumentSetDefinition definition,
            Action<DocumentSetModelNode> action)
            where TModelNode : ModelNode, IFolderHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddDocumentSets<TModelNode>(this TModelNode model, IEnumerable<DocumentSetDefinition> definitions)
           where TModelNode : ModelNode, IFolderHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
