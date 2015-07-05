using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ListModelNode : TypedModelNode, IListModelNode,
        IFieldHostModelNode,
        ISecurableObjectHostModelNode,
        IListItemHostModelNode,
        IFolderHostModelNode,
        IEventReceiverHostModelNode,
        IWelcomePageHostModelNode,
        IModuleFileHostModelNode,
        IAuditSettingsHostModelNode,
        IContentTypeLinkHostModelNode,
        IUserCustomActionHostModelNode,
        ISP2013WorkflowSubscriptionHostModelNode
    {

    }

    public static class ListDefinitionSyntax
    {
        #region methods

        public static TModelNode AddList<TModelNode>(this TModelNode model, ListDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddList(model, definition, null);
        }

        public static TModelNode AddList<TModelNode>(this TModelNode model, ListDefinition definition,
            Action<ListModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddLists<TModelNode>(this TModelNode model, IEnumerable<ListDefinition> definitions)
           where TModelNode : ModelNode, IWebModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion


        #region host override

        public static WebModelNode AddHostList(this WebModelNode model, ListDefinition definition)
        {
            return AddHostList(model, definition, null);
        }

        public static WebModelNode AddHostList(this WebModelNode model, ListDefinition definition, Action<ListModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
