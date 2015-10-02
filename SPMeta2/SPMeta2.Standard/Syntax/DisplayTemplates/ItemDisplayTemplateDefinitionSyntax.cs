using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class ItemDisplayTemplateModelNode : ListItemModelNode
    {

    }
    public static class ItemDisplayTemplateDefinitionSyntax
    {
        #region methods

        public static TModelNode AddItemDisplayTemplate<TModelNode>(this TModelNode model, ItemDisplayTemplateDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddItemDisplayTemplate(model, definition, null);
        }

        public static TModelNode AddItemDisplayTemplate<TModelNode>(this TModelNode model, ItemDisplayTemplateDefinition definition,
            Action<ItemDisplayTemplateModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddItemDisplayTemplates<TModelNode>(this TModelNode model, IEnumerable<ItemDisplayTemplateDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
