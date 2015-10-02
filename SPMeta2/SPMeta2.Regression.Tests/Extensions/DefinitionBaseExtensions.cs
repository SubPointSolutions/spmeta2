using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;

namespace SPMeta2.Regression.Tests.Extensions
{
    public static class DefinitionBaseExtensions
    {
        public static TDefinition As<TDefinition>(this DefinitionBase def)
            where TDefinition : DefinitionBase
        {
            return def as TDefinition;
        }

    }
}
