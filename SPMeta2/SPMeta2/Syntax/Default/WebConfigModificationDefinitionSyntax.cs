using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;

namespace SPMeta2.Syntax.Default
{
    public class WebConfigModificationModelNode : TypedModelNode
    {

    }

    public static class WebConfigModificationDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWebConfigModification<TModelNode>(this TModelNode model, WebConfigModificationDefinition definition)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return AddWebConfigModification(model, definition, null);
        }

        public static TModelNode AddWebConfigModification<TModelNode>(this TModelNode model, WebConfigModificationDefinition definition,
            Action<ModuleFileModelNode> action)
            where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddWebConfigModifications<TModelNode>(this TModelNode model, IEnumerable<WebConfigModificationDefinition> definitions)
           where TModelNode : ModelNode, IWebApplicationModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
