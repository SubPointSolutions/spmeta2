using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class OfficialFileHostModelNode : TypedModelNode
    {

    }

    public static class OfficialFileHostDefinitionSyntax
    {
        #region methods

        public static TModelNode AddOfficialFileHost<TModelNode>(this TModelNode model, OfficialFileHostDefinition definition)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return AddOfficialFileHost(model, definition, null);
        }

        public static TModelNode AddOfficialFileHost<TModelNode>(this TModelNode model, OfficialFileHostDefinition definition,
            Action<OfficialFileHostModelNode> action)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddOfficialFileHosts<TModelNode>(this TModelNode model, IEnumerable<OfficialFileHostDefinition> definitions)
           where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
