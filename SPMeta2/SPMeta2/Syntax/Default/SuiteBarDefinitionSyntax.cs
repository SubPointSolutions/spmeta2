using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class SuiteBarModelNode : TypedModelNode
    {

    }

    public static class SuiteBarDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSuiteBar<TModelNode>(this TModelNode model, SuiteBarDefinition definition)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return AddSuiteBar(model, definition, null);
        }

        public static TModelNode AddSuiteBar<TModelNode>(this TModelNode model, SuiteBarDefinition definition,
            Action<SuiteBarModelNode> action)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        //public static TModelNode AddOfficialFileHosts<TModelNode>(this TModelNode model, IEnumerable<OfficialFileHostDefinition> definitions)
        //   where TModelNode : ModelNode, IWebApplicationModelNode, new()
        //{
        //    foreach (var definition in definitions)
        //        model.AddDefinitionNode(definition);

        //    return model;
        //}

        #endregion
    }
}
