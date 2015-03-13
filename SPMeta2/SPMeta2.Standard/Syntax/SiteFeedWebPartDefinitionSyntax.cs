using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class SiteFeedWebPartDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddSiteFeedWebPart(this ModelNode model, SiteFeedWebPartDefinition definition)
        {
            return AddSiteFeedWebPart(model, definition, null);
        }

        public static ModelNode AddSiteFeedWebPart(this ModelNode model, SiteFeedWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddSiteFeedWebParts(this ModelNode model, IEnumerable<SiteFeedWebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
