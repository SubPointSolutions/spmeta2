using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class WorkflowAssociationModelNode : TypedModelNode
    {

    }

    public static class WorkflowAssociationDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWorkflowAssociation<TModelNode>(this TModelNode model, WorkflowAssociationDefinition definition)
            where TModelNode : ModelNode, IWorkflowAssociationHostModelNode, new()
        {
            return AddWorkflowAssociation(model, definition, null);
        }

        public static TModelNode AddWorkflowAssociation<TModelNode>(this TModelNode model, WorkflowAssociationDefinition definition,
            Action<WorkflowAssociationModelNode> action)
            where TModelNode : ModelNode, IWorkflowAssociationHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddWorkflowAssociations<TModelNode>(this TModelNode model, IEnumerable<WorkflowAssociationDefinition> definitions)
           where TModelNode : ModelNode, IWorkflowAssociationHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
