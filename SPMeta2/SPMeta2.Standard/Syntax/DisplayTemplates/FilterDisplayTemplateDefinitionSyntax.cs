using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class FilterDisplayTemplateModelNode : ListItemModelNode
    {

    }

    public static class FilterDisplayTemplateDefinitionSyntax
    {
        #region methods

        public static TModelNode AddFilterDisplayTemplate<TModelNode>(this TModelNode model, FilterDisplayTemplateDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddFilterDisplayTemplate(model, definition, null);
        }

        public static TModelNode AddFilterDisplayTemplate<TModelNode>(this TModelNode model, FilterDisplayTemplateDefinition definition,
            Action<FilterDisplayTemplateModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddFilterDisplayTemplates<TModelNode>(this TModelNode model, IEnumerable<FilterDisplayTemplateDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
