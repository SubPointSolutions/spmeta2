using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ModuleFileModelNode : TypedModelNode,
        ISecurableObjectHostModelNode
    {

    }

    public static class ModuleFileDefinitionSyntax
    {
        #region methods

        public static TModelNode AddModuleFile<TModelNode>(this TModelNode model, ModuleFileDefinition definition)
            where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return AddModuleFile(model, definition, null);
        }

        public static TModelNode AddModuleFile<TModelNode>(this TModelNode model, ModuleFileDefinition definition,
            Action<ModuleFileModelNode> action)
            where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddModuleFiles<TModelNode>(this TModelNode model, IEnumerable<ModuleFileDefinition> definitions)
           where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        public static TModelNode AddHostModuleFile<TModelNode>(this TModelNode model, ModuleFileDefinition definition)
          where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return AddHostModuleFile(model, definition, null);
        }
        public static TModelNode AddHostModuleFile<TModelNode>(this TModelNode model, ModuleFileDefinition definition,
            Action<ModuleFileModelNode> action)
            where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }
    }
}
