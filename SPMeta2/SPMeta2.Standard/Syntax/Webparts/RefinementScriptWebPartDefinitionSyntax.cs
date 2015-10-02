using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class RefinementScriptWebPartModelNode : WebPartModelNode
    {

    }
    public static class RefinementScriptWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddRefinementScriptWebPart<TModelNode>(this TModelNode model, RefinementScriptWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddRefinementScriptWebPart(model, definition, null);
        }

        public static TModelNode AddRefinementScriptWebPart<TModelNode>(this TModelNode model, RefinementScriptWebPartDefinition definition,
            Action<RefinementScriptWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddRefinementScriptWebParts<TModelNode>(this TModelNode model, IEnumerable<RefinementScriptWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
