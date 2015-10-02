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
    public class TargetApplicationModelNode : TypedModelNode
    {

    }

    public static class TargetApplicationDefinitionSyntax
    {
        #region methods

        public static TModelNode AddTargetApplication<TModelNode>(this TModelNode model, TargetApplicationDefinition definition)
            where TModelNode : ModelNode, ITargetApplicationHostModelNode, new()
        {
            return AddTargetApplication(model, definition, null);
        }

        public static TModelNode AddTargetApplication<TModelNode>(this TModelNode model, TargetApplicationDefinition definition,
            Action<TargetApplicationModelNode> action)
            where TModelNode : ModelNode, ITargetApplicationHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
