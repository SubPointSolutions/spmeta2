using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class EventReceiverDefinitionSyntax
    {
        #region methods

        public static SiteModelNode AddEventReceiver(this SiteModelNode model, EventReceiverDefinition definition)
        {
            return AddEventReceiver(model, definition, null);
        }

        public static SiteModelNode AddEventReceiver(this SiteModelNode model, EventReceiverDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddEventReceiver(this WebModelNode model, EventReceiverDefinition definition)
        {
            return AddEventReceiver(model, definition, null);
        }

        public static WebModelNode AddEventReceiver(this WebModelNode model, EventReceiverDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ListModelNode AddEventReceiver(this ListModelNode model, EventReceiverDefinition definition)
        {
            return AddEventReceiver(model, definition, null);
        }

        public static ListModelNode AddEventReceiver(this ListModelNode model, EventReceiverDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ContentTypeModelNode AddEventReceiver(this ContentTypeModelNode model, EventReceiverDefinition definition)
        {
            return AddEventReceiver(model, definition, null);
        }

        public static ContentTypeModelNode AddEventReceiver(this ContentTypeModelNode model, EventReceiverDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddEventReceivers(this ModelNode model, IEnumerable<EventReceiverDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
