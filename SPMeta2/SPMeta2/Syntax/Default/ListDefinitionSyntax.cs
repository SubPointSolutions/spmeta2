using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class ListModelNode : TypedModelNode
    {

    }

    public static class ListDefinitionSyntax
    {
        #region methods

        public static WebModelNode AddList(this WebModelNode model, ListDefinition definition)
        {
            return AddList(model, definition, null);
        }

        public static WebModelNode AddList(this WebModelNode model, ListDefinition definition, Action<ListModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static WebModelNode AddLists(this WebModelNode model, IEnumerable<ListDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region host override

        public static WebModelNode AddHostList(this WebModelNode model, ListDefinition definition)
        {
            return AddHostList(model, definition, null);
        }

        public static WebModelNode AddHostList(this WebModelNode model, ListDefinition definition, Action<ListModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
