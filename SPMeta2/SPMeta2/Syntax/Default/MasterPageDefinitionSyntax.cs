using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class MasterPageModelNode : ListItemModelNode
    {

    }

    public static class MasterPageDefinitionSyntax
    {
        #region methods

        public static TModelNode AddMasterPage<TModelNode>(this TModelNode model, MasterPageDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddMasterPage(model, definition, null);
        }

        public static TModelNode AddMasterPage<TModelNode>(this TModelNode model, MasterPageDefinition definition,
            Action<MasterPageModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddMasterPages<TModelNode>(this TModelNode model, IEnumerable<MasterPageDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
