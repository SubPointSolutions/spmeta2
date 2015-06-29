using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using System.IO;
using System.Reflection;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class ModuleFileModelNode : TypedModelNode
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

        public static ModelNode AddHostModuleFile(this ModelNode model, ModuleFileDefinition definition)
        {
            return AddHostModuleFile(model, definition, null);
        }

        public static ModelNode AddHostModuleFile(this ModelNode model, ModuleFileDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

    }
}
