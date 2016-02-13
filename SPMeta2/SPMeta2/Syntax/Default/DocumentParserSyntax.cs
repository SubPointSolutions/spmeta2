using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class DocumentParserModelNode : TypedModelNode
    {

    }


    public static class DocumentParserDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDocumentParser<TModelNode>(this TModelNode model, DocumentParserDefinition definition)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return AddDocumentParser(model, definition, null);
        }

        public static TModelNode AddDocumentParser<TModelNode>(this TModelNode model, DocumentParserDefinition definition,
            Action<DocumentParserModelNode> action)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddDocumentParsers<TModelNode>(this TModelNode model, IEnumerable<DocumentParserDefinition> definitions)
           where TModelNode : ModelNode, IFarmModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

    }
}
