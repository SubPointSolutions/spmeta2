using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class DiagnosticsServiceBaseModelNode : ListItemModelNode
    {

    }

    public static class DiagnosticsServiceBaseDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDiagnosticsServiceBase<TModelNode>(this TModelNode model, DiagnosticsServiceBaseDefinition definition)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return AddDiagnosticsServiceBase(model, definition, null);
        }

        public static TModelNode AddDiagnosticsServiceBase<TModelNode>(this TModelNode model, DiagnosticsServiceBaseDefinition definition,
            Action<DiagnosticsServiceBaseModelNode> action)
            where TModelNode : ModelNode, IFarmModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddDiagnosticsServiceBases<TModelNode>(this TModelNode model, IEnumerable<DiagnosticsServiceBaseDefinition> definitions)
           where TModelNode : ModelNode, IFarmModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
