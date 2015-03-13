using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class FolderDefinitionSyntax
    {
        #region add folders

        public static ModelNode AddFolder(this ModelNode model, FolderDefinition definition)
        {
            return AddFolder(model, definition, null);
        }

        public static ModelNode AddFolder(this ModelNode model, FolderDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload
        public static ModelNode AddFolders(this ModelNode model, IEnumerable<FolderDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region add host

        public static ModelNode AddHostFolder(this ModelNode model, FolderDefinition definition)
        {
            return AddHostFolder(model, definition, null);
        }

        public static ModelNode AddHostFolder(this ModelNode model, FolderDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
