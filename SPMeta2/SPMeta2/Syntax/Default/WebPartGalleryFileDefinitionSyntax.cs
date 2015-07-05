using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class WebPartGalleryFileModelNode : TypedModelNode
    {

    }

    public static class WebPartGalleryFileDefinitionSyntax
    {
        #region methods

        public static TModelNode AddWebPartGalleryFile<TModelNode>(this TModelNode model, WebPartGalleryFileDefinition definition)
            where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return AddWebPartGalleryFile(model, definition, null);
        }

        public static TModelNode AddWebPartGalleryFile<TModelNode>(this TModelNode model, WebPartGalleryFileDefinition definition,
            Action<WebPartGalleryFileModelNode> action)
            where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddWebPartGalleryFiles<TModelNode>(this TModelNode model, IEnumerable<WebPartGalleryFileDefinition> definitions)
           where TModelNode : ModelNode, IModuleFileHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
