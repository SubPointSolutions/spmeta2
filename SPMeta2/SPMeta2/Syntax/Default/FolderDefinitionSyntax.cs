using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class FolderModelNode : TypedModelNode,
        ISecurableObjectHostModelNode,
        IListItemHostModelNode,
        IFolderHostModelNode,
        IWelcomePageHostModelNode,
        IModuleFileHostModelNode,
        IPropertyHostModelNode
    {

    }

    public static class FolderDefinitionSyntax
    {
        #region add folders

        #region methods

        public static TModelNode AddFolder<TModelNode>(this TModelNode model, FolderDefinition definition)
            where TModelNode : ModelNode, IFolderHostModelNode, new()
        {
            return AddFolder(model, definition, null);
        }

        public static TModelNode AddFolder<TModelNode>(this TModelNode model, FolderDefinition definition,
            Action<FolderModelNode> action)
            where TModelNode : ModelNode, IFolderHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddFolders<TModelNode>(this TModelNode model, IEnumerable<FolderDefinition> definitions)
           where TModelNode : ModelNode, IFolderHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #endregion

        #region array overload
        //public static ModelNode AddFolders(this ModelNode model, IEnumerable<FolderDefinition> definitions)
        //{
        //    foreach (var definition in definitions)
        //        model.AddDefinitionNode(definition);

        //    return model;
        //}

        #endregion

        #region add host

        public static TModelNode AddHostFolder<TModelNode>(this TModelNode model, FolderDefinition definition)
           where TModelNode : ModelNode, IFolderHostModelNode, new()
        {
            return AddHostFolder(model, definition, null);
        }
        public static TModelNode AddHostFolder<TModelNode>(this TModelNode model, FolderDefinition definition,
            Action<FolderModelNode> action)
            where TModelNode : ModelNode, IFolderHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
