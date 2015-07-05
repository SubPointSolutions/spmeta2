using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
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
    public class UserFieldModelNode : FieldModelNode
    {

    }

    public static class UserFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddUserField<TModelNode>(this TModelNode model, UserFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddUserField(model, definition, null);
        }

        public static TModelNode AddUserField<TModelNode>(this TModelNode model, UserFieldDefinition definition,
            Action<UserFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddUserFields<TModelNode>(this TModelNode model, IEnumerable<UserFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
