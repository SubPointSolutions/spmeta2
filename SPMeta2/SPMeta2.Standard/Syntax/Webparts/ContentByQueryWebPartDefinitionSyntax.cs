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
    public class ContentByQueryWebPartModelNode : WebPartModelNode
    {

    }

    public static class ContentByQueryWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddContentByQueryWebPart<TModelNode>(this TModelNode model, ContentByQueryWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddContentByQueryWebPart(model, definition, null);
        }

        public static TModelNode AddContentByQueryWebPart<TModelNode>(this TModelNode model, ContentByQueryWebPartDefinition definition,
            Action<ContentByQueryWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddContentByQueryWebParts<TModelNode>(this TModelNode model, IEnumerable<ContentByQueryWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
