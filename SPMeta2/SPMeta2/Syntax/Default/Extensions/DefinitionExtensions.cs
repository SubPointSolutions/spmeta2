using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;

namespace SPMeta2.Syntax.Default
{
    public static class DefinitionExtensions
    {
        public static TDefinition Inherit<TDefinition>(this TDefinition definition)
            where TDefinition : DefinitionBase, new()
        {
            return Inherit<TDefinition>(definition, null);
        }

        public static TDefinition Inherit<TDefinition>(this TDefinition definition, Action<TDefinition> config)
            where TDefinition : DefinitionBase, new()
        {
            var model = definition.Clone() as TDefinition;

            if (config != null)
                config(model);

            return model;
        }
    }
}
