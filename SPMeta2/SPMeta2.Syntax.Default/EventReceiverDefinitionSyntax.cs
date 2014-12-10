using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class EventReceiverDefinitionSyntax
    {
        #region methods

        public static ModelNode AddEventReceiver(this ModelNode model, EventReceiverDefinition definition)
        {
            return AddEventReceiver(model, definition, null);
        }

        public static ModelNode AddEventReceiver(this ModelNode model, EventReceiverDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
