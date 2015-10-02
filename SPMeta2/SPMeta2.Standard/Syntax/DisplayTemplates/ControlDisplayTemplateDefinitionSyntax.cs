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
    public class ControlDisplayTemplateModelNode : ListItemModelNode
    {

    }

    public static class ControlDisplayTemplateDefinitionSyntax
    {
        #region methods

        public static TModelNode AddControlDisplayTemplate<TModelNode>(this TModelNode model, ControlDisplayTemplateDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddControlDisplayTemplate(model, definition, null);
        }

        public static TModelNode AddControlDisplayTemplate<TModelNode>(this TModelNode model, ControlDisplayTemplateDefinition definition,
            Action<ControlDisplayTemplateModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddControlDisplayTemplates<TModelNode>(this TModelNode model, IEnumerable<ControlDisplayTemplateDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
