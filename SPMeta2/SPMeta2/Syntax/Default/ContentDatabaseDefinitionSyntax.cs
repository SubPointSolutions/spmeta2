using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ContentDatabaseModelNode : TypedModelNode
    {

    }

    public static class ContentDatabaseDefinitionSyntax
    {
        #region methods

        public static TModelNode AddContentDatabase<TModelNode>(this TModelNode model, ContentDatabaseDefinition definition)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return AddContentDatabase(model, definition, null);
        }

        public static TModelNode AddContentDatabase<TModelNode>(this TModelNode model, ContentDatabaseDefinition definition,
            Action<ContentDatabaseModelNode> action)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddContentDatabases<TModelNode>(this TModelNode model, IEnumerable<ContentDatabaseDefinition> definitions)
           where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
