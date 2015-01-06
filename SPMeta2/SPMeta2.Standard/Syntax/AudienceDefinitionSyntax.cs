using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    public static class AudienceDefinitionSyntax
    {
        #region publishing page

        public static ModelNode AddAudience(this ModelNode model, AudienceDefinition definition)
        {
            return AddAudience(model, definition, null);
        }

        public static ModelNode AddAudience(this ModelNode model, AudienceDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion
    }
}
