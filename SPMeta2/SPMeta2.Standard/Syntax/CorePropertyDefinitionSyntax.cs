using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class CorePropertyModelNode : TypedModelNode,
        IProfileTypePropertyHostModelNode
    {

    }

    public static class CorePropertyDefinitionSyntax
    {
        #region methods

        public static TModelNode AddCoreProperty<TModelNode>(this TModelNode model, CorePropertyDefinition definition)
            where TModelNode : ModelNode, ICorePropertyHostModelNode, new()
        {
            return AddCoreProperty(model, definition, null);
        }

        public static TModelNode AddCoreProperty<TModelNode>(this TModelNode model, CorePropertyDefinition definition,
            Action<CorePropertyModelNode> action)
            where TModelNode : ModelNode, ICorePropertyHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddCoreProperties<TModelNode>(this TModelNode model, IEnumerable<CorePropertyDefinition> definitions)
           where TModelNode : ModelNode, ICorePropertyHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
