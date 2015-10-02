using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class ImageRenditionModelNode : ListItemModelNode
    {

    }

    public static class ImageRenditionDefinitionSyntax
    {
        #region methods

        public static TModelNode AddImageRendition<TModelNode>(this TModelNode model, ImageRenditionDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddImageRendition(model, definition, null);
        }

        public static TModelNode AddImageRendition<TModelNode>(this TModelNode model, ImageRenditionDefinition definition,
            Action<ImageRenditionModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddImageRenditions<TModelNode>(this TModelNode model, IEnumerable<ImageRenditionDefinition> definitions)
           where TModelNode : ModelNode, ISiteModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
