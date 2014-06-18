using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Models;
using System.IO;
using System.Reflection;

namespace SPMeta2.Syntax.Default
{
    public static class ModuleFileDefinitionSyntax
    {
        public static ModelNode AddModuleFile(this ModelNode model, ModuleFileDefinition moduleFileDefinition)
        {
            return AddModuleFile(model, moduleFileDefinition, null);
        }

        public static ModelNode AddModuleFile(this ModelNode model, ModuleFileDefinition moduleFileDefinition, Action<ModelNode> action)
        {
            var newModelNode = new ModelNode { Value = moduleFileDefinition };

            model.ChildModels.Add(newModelNode);

            if (action != null) action(newModelNode);

            return model;
        }      

    }
}
