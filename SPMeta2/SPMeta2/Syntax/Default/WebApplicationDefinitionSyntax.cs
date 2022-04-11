using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class WebApplicationModelNode : TypedModelNode, IWebApplicationModelNode,
        IPropertyHostModelNode,
        IFeatureHostModelNode,
        IJobHostModelNode,
        ISiteHostModelNode,
        IFarmSolutionModelHostNode
    {

    }

    public static class WebApplicationDefinitionSyntax
    {
        #region add host

        public static TModelNode AddHostWebApplication<TModelNode>(this TModelNode model, WebApplicationDefinition definition)
             where TModelNode : ModelNode, IWebApplicationHostModelNode, new()
        {
            return AddHostWebApplication(model, definition, null);
        }
        public static TModelNode AddHostWebApplication<TModelNode>(this TModelNode model, WebApplicationDefinition definition,
            Action<WebApplicationModelNode> action)
            where TModelNode : ModelNode, IWebApplicationHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion

        #region methods

        public static TModelNode AddWebApplication<TModelNode>(this TModelNode model, WebApplicationDefinition definition)
            where TModelNode : ModelNode, IWebApplicationHostModelNode, new()
        {
            return AddWebApplication(model, definition, null);
        }

        public static TModelNode AddWebApplication<TModelNode>(this TModelNode model, WebApplicationDefinition definition,
            Action<WebApplicationModelNode> action)
            where TModelNode : ModelNode, IWebApplicationHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddWebApplications<TModelNode>(this TModelNode model, IEnumerable<WebApplicationDefinition> definitions)
           where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
