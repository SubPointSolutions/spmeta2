using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class SiteDefinitionSyntax
    {
        #region methods

        public static ModelNode AddSite(this ModelNode model, SiteDefinition definition)
        {
            return AddSite(model, definition, null);
        }

        public static ModelNode AddSite(this ModelNode model, SiteDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        #endregion

        #region add host

        public static ModelNode AddHostSite(this ModelNode model, SiteDefinition definition)
        {
            return AddHostSite(model, definition, null);
        }

        public static ModelNode AddHostSite(this ModelNode model, SiteDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
