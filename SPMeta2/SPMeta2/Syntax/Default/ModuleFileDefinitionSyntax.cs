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
        public static WebModelNode AddModuleFile(this WebModelNode model, ModuleFileDefinition definition)
        {
            return AddModuleFile(model, definition, null);
        }

        public static WebModelNode AddModuleFile(this WebModelNode model, ModuleFileDefinition definition,
            Action<ModuleFileModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ContentTypeModelNode AddModuleFile(this ContentTypeModelNode model, ModuleFileDefinition definition)
        {
            return AddModuleFile(model, definition, null);
        }

        public static ContentTypeModelNode AddModuleFile(this ContentTypeModelNode model, ModuleFileDefinition definition,
            Action<ModuleFileModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ListModelNode AddModuleFile(this ListModelNode model, ModuleFileDefinition definition)
        {
            return AddModuleFile(model, definition, null);
        }

        public static ListModelNode AddModuleFile(this ListModelNode model, ModuleFileDefinition definition,
            Action<ModuleFileModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }


        #region array overload

        public static ModelNode AddModuleFiles(this ModelNode model, IEnumerable<ModuleFileDefinition> definitions)
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
