using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class DocumentParserModelNode : ListItemModelNode
    {

    }

    public static class DocumentParserDefinitionSyntax
    {
        #region methods

        public static FarmModelNode AddDocumentParser(this FarmModelNode model, DocumentParserDefinition definition)
        {
            return AddDocumentParser(model, definition, null);
        }

        public static FarmModelNode AddDocumentParser(this FarmModelNode model, DocumentParserDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
