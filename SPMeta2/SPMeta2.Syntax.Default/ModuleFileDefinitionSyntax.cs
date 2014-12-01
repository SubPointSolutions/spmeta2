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
    public static class ModuleFileDefinitionSyntax
    {
        public static ModelNode AddModuleFile(this ModelNode model, ModuleFileDefinition definition)
        {
            return AddModuleFile(model, definition, null);
        }

        public static ModelNode AddModuleFile(this ModelNode model, ModuleFileDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

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
