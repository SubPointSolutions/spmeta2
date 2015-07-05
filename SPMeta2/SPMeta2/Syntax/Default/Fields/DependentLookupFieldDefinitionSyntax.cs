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
    public class DependentLookupFieldModelNode : FieldModelNode
    {

    }

    public static class DependentLookupFieldDefinitionSyntax
    {
        #region methods

        public static TModelNode AddDependentLookupField<TModelNode>(this TModelNode model, DependentLookupFieldDefinition definition)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return AddDependentLookupField(model, definition, null);
        }

        public static TModelNode AddDependentLookupField<TModelNode>(this TModelNode model, DependentLookupFieldDefinition definition,
            Action<DependentLookupFieldModelNode> action)
            where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddDependentLookupFields<TModelNode>(this TModelNode model, IEnumerable<DependentLookupFieldDefinition> definitions)
           where TModelNode : ModelNode, IFieldHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
