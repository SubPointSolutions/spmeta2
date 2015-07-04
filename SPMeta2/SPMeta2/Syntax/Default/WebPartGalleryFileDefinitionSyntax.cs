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
    public class WebPartGalleryFileModelNode : TypedModelNode
    {

    }

    public static class WebPartGalleryFileDefinitionSyntax
    {
        #region methods

        public static ListModelNode AddWebPartGalleryFile(this ListModelNode model, WebPartGalleryFileDefinition definition)
        {
            return AddWebPartGalleryFile(model, definition, null);
        }

        public static ListModelNode AddWebPartGalleryFile(this ListModelNode model, WebPartGalleryFileDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ListModelNode AddWebPartGalleryFiles(this ListModelNode model, IEnumerable<WebPartGalleryFileDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
