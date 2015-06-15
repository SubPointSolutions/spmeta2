using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class SiteModelNode : TypedModelNode
    {

    }

    public static class SiteDefinitionSyntax
    {
        #region methods



        public static WebApplicationModelNode AddSite(this WebApplicationModelNode model, SiteDefinition definition)
        {
            return AddSite(model, definition, null);
        }

        public static WebApplicationModelNode AddSite(this WebApplicationModelNode model, SiteDefinition definition, Action<SiteModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region add host

        public static WebApplicationModelNode AddHostSite(this WebApplicationModelNode model, SiteDefinition definition)
        {
            return AddHostSite(model, definition, null);
        }

        public static WebApplicationModelNode AddHostSite(this WebApplicationModelNode model, SiteDefinition definition, Action<SiteModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
