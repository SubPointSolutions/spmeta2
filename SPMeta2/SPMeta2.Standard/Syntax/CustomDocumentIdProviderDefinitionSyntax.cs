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
    public class CustomDocumentIdProviderModelNode : ListItemModelNode
    {

    }

    public static class CustomDocumentIdProviderDefinitionSyntax
    {
        #region methods

        public static TModelNode AddCustomDocumentIdProvider<TModelNode>(this TModelNode model, CustomDocumentIdProviderDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddCustomDocumentIdProvider(model, definition, null);
        }

        public static TModelNode AddCustomDocumentIdProvider<TModelNode>(this TModelNode model, CustomDocumentIdProviderDefinition definition,
            Action<CustomDocumentIdProviderModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
