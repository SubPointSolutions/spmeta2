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
    public class FilterDisplayTemplateModelNode : ListItemModelNode
    {

    }

    public static class FilterDisplayTemplateDefinitionSyntax
    {
        #region add

        public static ListModelNode AddFilterDisplayTemplate(this ListModelNode model, FilterDisplayTemplateDefinition definition)
        {
            return AddFilterDisplayTemplate(model, definition, null);
        }

        public static ListModelNode AddFilterDisplayTemplate(this ListModelNode model, FilterDisplayTemplateDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        public static ModelNode AddFilterDisplayTemplates(this ModelNode model, IEnumerable<FilterDisplayTemplateDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

    }
}
