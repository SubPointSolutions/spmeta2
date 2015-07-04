using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public class ItemDisplayTemplateModelNode : ListItemModelNode
    {

    }
    public static class ItemDisplayTemplateDefinitionSyntax
    {
        #region publishing page

        public static ListModelNode AddItemDisplayTemplate(this ListModelNode model, ItemDisplayTemplateDefinition definition)
        {
            return AddItemDisplayTemplate(model, definition, null);
        }

        public static ListModelNode AddItemDisplayTemplate(this ListModelNode model, ItemDisplayTemplateDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddItemDisplayTemplates(this ModelNode model, IEnumerable<ItemDisplayTemplateDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
