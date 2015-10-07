using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SP2013WorkflowSubscriptionModelNode : TypedModelNode
    {

    }

    public static class SP2013WorkflowSubscriptionDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSP2013WorkflowSubscription<TModelNode>(this TModelNode model, SP2013WorkflowSubscriptionDefinition definition)
            where TModelNode : ModelNode, ISP2013WorkflowSubscriptionHostModelNode, new()
        {
            return AddSP2013WorkflowSubscription(model, definition, null);
        }

        public static TModelNode AddSP2013WorkflowSubscription<TModelNode>(this TModelNode model, SP2013WorkflowSubscriptionDefinition definition,
            Action<SP2013WorkflowSubscriptionModelNode> action)
            where TModelNode : ModelNode, ISP2013WorkflowSubscriptionHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSP2013WorkflowSubscriptions<TModelNode>(this TModelNode model, IEnumerable<SP2013WorkflowSubscriptionDefinition> definitions)
           where TModelNode : ModelNode, ISP2013WorkflowSubscriptionHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
