using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SP2013WorkflowModelNode : TypedModelNode
    {

    }

    public static class SP2013WorkflowDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSP2013Workflow<TModelNode>(this TModelNode model, SP2013WorkflowDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddSP2013Workflow(model, definition, null);
        }

        public static TModelNode AddSP2013Workflow<TModelNode>(this TModelNode model, SP2013WorkflowDefinition definition,
            Action<SP2013WorkflowModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSP2013Workflows<TModelNode>(this TModelNode model, IEnumerable<SP2013WorkflowDefinition> definitions)
           where TModelNode : ModelNode, IWebModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
