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
    public class SearchConfigurationModelNode : TypedModelNode
    {

    }

    public static class SearchConfigurationDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSearchConfiguration<TModelNode>(this TModelNode model, SearchConfigurationDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddSearchConfiguration(model, definition, null);
        }

        public static TModelNode AddSearchConfiguration<TModelNode>(this TModelNode model, SearchConfigurationDefinition definition,
            Action<SearchConfigurationModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
