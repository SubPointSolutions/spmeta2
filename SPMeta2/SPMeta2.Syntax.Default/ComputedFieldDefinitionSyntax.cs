using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class ComputedFieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddComputedField(this ModelNode model, ComputedFieldDefinition definition)
        {
            return AddComputedField(model, definition, null);
        }

        public static ModelNode AddComputedField(this ModelNode model, ComputedFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddComputedFields(this ModelNode model, IEnumerable<ComputedFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

    }
}
