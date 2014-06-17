using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class UserCustomActionSyntax
    {
        #region methods

        public static ModelNode AddUserCustomAction(this ModelNode model, UserCustomActionDefinition customAction)
        {
            return AddUserCustomAction(model, customAction, null);
        }

        public static ModelNode AddUserCustomAction(this ModelNode model, UserCustomActionDefinition customAction, Action<ModelNode> action)
        {
            var newModel = new ModelNode { Value = customAction };

            model.ChildModels.Add(newModel);

            if (action != null)
                action(newModel);

            return model;
        }

        #endregion
    }
}
