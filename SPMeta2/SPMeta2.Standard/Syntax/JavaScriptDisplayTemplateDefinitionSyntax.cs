using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using System.Collections.Generic;

namespace SPMeta2.Standard.Syntax
{
    public static class JavaScriptDisplayTemplateDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddJavaScriptDisplayTemplate(this ModelNode model, JavaScriptDisplayTemplateDefinition definition)
        {
            return AddJavaScriptDisplayTemplate(model, definition, null);
        }

        public static ModelNode AddJavaScriptDisplayTemplate(this ModelNode model, JavaScriptDisplayTemplateDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        public static ModelNode AddJavaScriptDisplayTemplates(this ModelNode model, IEnumerable<JavaScriptDisplayTemplateDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

    }
}
