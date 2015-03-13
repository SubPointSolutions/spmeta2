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
    public static class BooleanFieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddBooleanField(this ModelNode model, BooleanFieldDefinition definition)
        {
            return AddBooleanField(model, definition, null);
        }

        public static ModelNode AddBooleanField(this ModelNode model, BooleanFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddBooleanFields(this ModelNode model, IEnumerable<BooleanFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
