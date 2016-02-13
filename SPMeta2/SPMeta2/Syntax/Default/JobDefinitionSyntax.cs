using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class JobModelNode : TypedModelNode
    {

    }

    public static class JobDefinitionSyntax
    {
        #region methods

        public static TModelNode AddJob<TModelNode>(this TModelNode model, JobDefinition definition)
            where TModelNode : ModelNode, IJobHostModelNode, new()
        {
            return AddJob(model, definition, null);
        }

        public static TModelNode AddJob<TModelNode>(this TModelNode model, JobDefinition definition,
            Action<JobModelNode> action)
            where TModelNode : ModelNode, IJobHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddJobs<TModelNode>(this TModelNode model, IEnumerable<JobDefinition> definitions)
           where TModelNode : ModelNode, IJobHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
