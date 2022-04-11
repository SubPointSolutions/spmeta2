using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

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
        ISP2013WorkflowSubscriptionHostModelNode,
        IWorkflowAssociationHostModelNode,
        IPropertyHostModelNode
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

        public static TModelNode AddHostList<TModelNode>(this TModelNode model, ListDefinition definition)
           where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddHostList(model, definition, null);
        }
        public static TModelNode AddHostList<TModelNode>(this TModelNode model, ListDefinition definition,
            Action<ListModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
