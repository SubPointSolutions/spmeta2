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
    public class URLFieldModelNode : FieldModelNode
    {

    }

    public static class URLFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddURLField<TModelNode>(this TModelNode model, URLFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddURLField(model, definition, null);
        }

        public static TModelNode AddURLField<TModelNode>(this TModelNode model, URLFieldDefinition definition,
            Action<URLFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddURLFields<TModelNode>(this TModelNode model, IEnumerable<URLFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
