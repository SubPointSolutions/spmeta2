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
    public static class TextFieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddTextField(this ModelNode model, TextFieldDefinition definition)
        {
            return AddTextField(model, definition, null);
        }

        public static ModelNode AddTextField(this ModelNode model, TextFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddTextFields(this ModelNode model, IEnumerable<TextFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
