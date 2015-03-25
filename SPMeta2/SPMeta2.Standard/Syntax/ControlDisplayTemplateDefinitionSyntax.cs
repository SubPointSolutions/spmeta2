using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class ControlDisplayTemplateDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddControlDisplayTemplate(this ModelNode model, ControlDisplayTemplateDefinition definition)
        {
            return AddControlDisplayTemplate(model, definition, null);
        }

        public static ModelNode AddControlDisplayTemplate(this ModelNode model, ControlDisplayTemplateDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddControlDisplayTemplates(this ModelNode model, IEnumerable<ControlDisplayTemplateDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
