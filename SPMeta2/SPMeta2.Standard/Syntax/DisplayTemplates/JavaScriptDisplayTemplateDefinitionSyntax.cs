using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using System.Collections.Generic;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Standard.Syntax
{
    public class JavaScriptDisplayTemplateModelNode : ListItemModelNode
    {

    }

    public static class JavaScriptDisplayTemplateDefinitionSyntax
    {
        #region methods

        public static TModelNode AddJavaScriptDisplayTemplate<TModelNode>(this TModelNode model, JavaScriptDisplayTemplateDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddJavaScriptDisplayTemplate(model, definition, null);
        }

        public static TModelNode AddJavaScriptDisplayTemplate<TModelNode>(this TModelNode model, JavaScriptDisplayTemplateDefinition definition,
            Action<ControlDisplayTemplateModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddJavaScriptDisplayTemplates<TModelNode>(this TModelNode model, IEnumerable<JavaScriptDisplayTemplateDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
