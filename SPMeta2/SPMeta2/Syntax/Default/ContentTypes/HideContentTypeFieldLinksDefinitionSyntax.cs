using System;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class HideContentTypeFieldLinksModelNode : ListItemModelNode
    {

    }

    public static class HideContentTypeFieldLinksDefinitionSyntax
    {
        #region methods

        public static ContentTypeModelNode AddHideContentTypeFieldLinks(this ContentTypeModelNode model, HideContentTypeFieldLinksDefinition definition)
        {
            return AddHideContentTypeFieldLinks(model, definition, null);
        }

        public static ContentTypeModelNode AddHideContentTypeFieldLinks(this ContentTypeModelNode model, HideContentTypeFieldLinksDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
