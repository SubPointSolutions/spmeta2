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
    public class ListViewWebPartModelNode : WebPartModelNode
    {

    }

    public static class ListViewWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddListViewWebPart<TModelNode>(this TModelNode model, ListViewWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddListViewWebPart(model, definition, null);
        }

        public static TModelNode AddListViewWebPart<TModelNode>(this TModelNode model, ListViewWebPartDefinition definition,
            Action<ListViewWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddListViewWebParts<TModelNode>(this TModelNode model, IEnumerable<ListViewWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
