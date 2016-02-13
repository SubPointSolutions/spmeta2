using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ListFieldLinkModelNode : ListItemModelNode
    {

    }

    public static class ListFieldLinkDefinitionSyntax
    {
        #region methods

        public static TModelNode AddListFieldLink<TModelNode>(this TModelNode model, FieldDefinition definition)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return AddListFieldLink(model, definition, null);
        }

        public static TModelNode AddListFieldLink<TModelNode>(this TModelNode model, FieldDefinition definition,
            Action<ListFieldLinkModelNode> action)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            if (definition.Id != default(Guid))
            {
                return model.AddListFieldLink(new ListFieldLinkDefinition
                {
                    FieldId = definition.Id
                }, action);
            }

            return model.AddListFieldLink(new ListFieldLinkDefinition
            {
                FieldInternalName = definition.InternalName
            }, action);
        }

        #endregion

        #region methods

        public static TModelNode AddListFieldLink<TModelNode>(this TModelNode model, ListFieldLinkDefinition definition)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return AddListFieldLink(model, definition, null);
        }

        public static TModelNode AddListFieldLink<TModelNode>(this TModelNode model, ListFieldLinkDefinition definition,
            Action<ListFieldLinkModelNode> action)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddListFieldLinks<TModelNode>(this TModelNode model, IEnumerable<ListFieldLinkDefinition> definitions)
           where TModelNode : ModelNode, IListModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
