using System;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class SearchSettingsModelNode : TypedModelNode
    {

    }

    public static class SearchSettingsDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSearchSettings<TModelNode>(this TModelNode model, SearchSettingsDefinition definition)
            where TModelNode : ModelNode, ISearchSettingsHostModelNode, new()
        {
            return AddSearchSettings(model, definition, null);
        }

        public static TModelNode AddSearchSettings<TModelNode>(this TModelNode model, SearchSettingsDefinition definition,
            Action<SearchSettingsModelNode> action)
            where TModelNode : ModelNode, ISearchSettingsHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
