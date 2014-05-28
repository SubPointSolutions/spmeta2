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
            model.ChildModels.Add(new ModelNode { Value = customAction });

            return model;
        }

        #endregion
    }
}
