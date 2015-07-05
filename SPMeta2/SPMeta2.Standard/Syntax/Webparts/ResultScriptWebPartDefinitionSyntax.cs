using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class ResultScriptWebPartModelNode : WebPartModelNode
    {

    }

    public static class ResultScriptWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddResultScriptWebPart<TModelNode>(this TModelNode model, ResultScriptWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddResultScriptWebPart(model, definition, null);
        }

        public static TModelNode AddResultScriptWebPart<TModelNode>(this TModelNode model, ResultScriptWebPartDefinition definition,
            Action<ResultScriptWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddResultScriptWebParts<TModelNode>(this TModelNode model, IEnumerable<ResultScriptWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
