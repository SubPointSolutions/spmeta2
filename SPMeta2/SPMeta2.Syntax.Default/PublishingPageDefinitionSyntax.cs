using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class PublishingPageDefinitionSyntax
    {
        public static ModelNode AddPublishingPage(this ModelNode model, PublishingPageDefinition definition)
        {
            return AddPublishingPage(model, definition, null);
        }

        public static ModelNode AddPublishingPage(this ModelNode model, PublishingPageDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }
    }
}
