using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Syntax.Default;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class SearchResultModelNode : TypedModelNode
    {

    }

    public static class SearchResultDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSearchResult<TModelNode>(this TModelNode model, SearchResultDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddSearchResult(model, definition, null);
        }

        public static TModelNode AddSearchResult<TModelNode>(this TModelNode model, SearchResultDefinition definition,
            Action<SearchResultModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSearchResults<TModelNode>(this TModelNode model, IEnumerable<SearchResultDefinition> definitions)
           where TModelNode : ModelNode, ISiteModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
