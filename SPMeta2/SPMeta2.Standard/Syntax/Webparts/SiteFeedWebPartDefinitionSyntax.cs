using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class SiteFeedWebPartModelNode : WebPartModelNode
    {

    }

    public static class SiteFeedWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSiteFeedWebPart<TModelNode>(this TModelNode model, SiteFeedWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddSiteFeedWebPart(model, definition, null);
        }

        public static TModelNode AddSiteFeedWebPart<TModelNode>(this TModelNode model, SiteFeedWebPartDefinition definition,
            Action<SiteFeedWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSiteFeedWebParts<TModelNode>(this TModelNode model, IEnumerable<SiteFeedWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
