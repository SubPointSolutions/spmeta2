using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    public static class FolderDefinitionSyntax
    {
        #region add folders

        public static ModelNode AddFolder(this ModelNode model, FolderDefinition folderModel)
        {
            return AddFolder(model, folderModel, null);
        }

        public static ModelNode AddFolder(this ModelNode model, FolderDefinition folderModel, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = folderModel };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }

        #endregion
    }
}
