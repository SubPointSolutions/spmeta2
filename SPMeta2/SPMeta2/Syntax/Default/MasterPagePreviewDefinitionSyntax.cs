using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
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
    public class MasterPagePreviewModelNode : ListItemModelNode
    {

    }

    public static class MasterPagePreviewDefinitionSyntax
    {
        #region methods

        public static TModelNode AddMasterPagePreview<TModelNode>(this TModelNode model, MasterPagePreviewDefinition definition)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return AddMasterPagePreview(model, definition, null);
        }

        public static TModelNode AddMasterPagePreview<TModelNode>(this TModelNode model, MasterPagePreviewDefinition definition,
            Action<MasterPagePreviewModelNode> action)
            where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddMasterPagePreviews<TModelNode>(this TModelNode model, IEnumerable<MasterPagePreviewDefinition> definitions)
           where TModelNode : ModelNode, IListItemHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
