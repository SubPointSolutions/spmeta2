using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class UserCustomActionModelNode : TypedModelNode
    {

    }

    public static class UserCustomActionSyntax
    {
        #region methods

        public static SiteModelNode AddUserCustomAction(this SiteModelNode model, UserCustomActionDefinition definition)
        {
            return AddUserCustomAction(model, definition, null);
        }

        public static SiteModelNode AddUserCustomAction(this SiteModelNode model, UserCustomActionDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddUserCustomAction(this WebModelNode model, UserCustomActionDefinition definition)
        {
            return AddUserCustomAction(model, definition, null);
        }

        public static WebModelNode AddUserCustomAction(this WebModelNode model, UserCustomActionDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }


        public static ListModelNode AddUserCustomAction(this ListModelNode model, UserCustomActionDefinition definition)
        {
            return AddUserCustomAction(model, definition, null);
        }

        public static ListModelNode AddUserCustomAction(this ListModelNode model, UserCustomActionDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }



        #endregion

        #region array overload

        public static ModelNode AddUserCustomActions(this ModelNode model, IEnumerable<UserCustomActionDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
