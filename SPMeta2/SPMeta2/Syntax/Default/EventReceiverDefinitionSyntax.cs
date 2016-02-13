using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class EventReceiverModelNode : TypedModelNode
    {

    }

    public static class EventReceiverDefinitionSyntax
    {
        #region methods

        public static TModelNode AddEventReceiver<TModelNode>(this TModelNode model, EventReceiverDefinition definition)
            where TModelNode : ModelNode, IEventReceiverHostModelNode, new()
        {
            return AddEventReceiver(model, definition, null);
        }

        public static TModelNode AddEventReceiver<TModelNode>(this TModelNode model, EventReceiverDefinition definition,
            Action<EventReceiverModelNode> action)
            where TModelNode : ModelNode, IEventReceiverHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddEventReceivers<TModelNode>(this TModelNode model, IEnumerable<EventReceiverDefinition> definitions)
           where TModelNode : ModelNode, IEventReceiverHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
