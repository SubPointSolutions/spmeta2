using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class AlternateUrlDefinitionSyntax
    {
        #region base

        public static ModelNode AddAlternateUrl(this ModelNode model, AlternateUrlDefinition definition)
        {
            return AddAlternateUrl(model, definition, null);
        }

        public static ModelNode AddAlternateUrl(this ModelNode model, AlternateUrlDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddAlternateUrls(this ModelNode model, IEnumerable<AlternateUrlDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
