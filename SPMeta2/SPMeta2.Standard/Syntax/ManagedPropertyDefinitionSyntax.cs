using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class ManagedPropertyModelNode : TypedModelNode
    {

    }

    public static class ManagedPropertyDefinitionSyntax
    {
        #region publishing page

        #region methods

        public static TModelNode AddManagedProperty<TModelNode>(this TModelNode model, ManagedPropertyDefinition definition)
            where TModelNode : ModelNode, IManagedPropertyHostModelNode, new()
        {
            return AddManagedProperty(model, definition, null);
        }

        public static TModelNode AddManagedProperty<TModelNode>(this TModelNode model, ManagedPropertyDefinition definition,
            Action<ManagedPropertyModelNode> action)
            where TModelNode : ModelNode, IManagedPropertyHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddManagedProperties<TModelNode>(this TModelNode model, IEnumerable<ManagedPropertyDefinition> definitions)
           where TModelNode : ModelNode, IManagedPropertyHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #endregion
    }
}
