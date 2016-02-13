using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class SilverlightWebPartModelNode : WebPartModelNode
    {

    }

    public static class SilverlightWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSilverlightWebPart<TModelNode>(this TModelNode model, SilverlightWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddSilverlightWebPart(model, definition, null);
        }

        public static TModelNode AddSilverlightWebPart<TModelNode>(this TModelNode model, SilverlightWebPartDefinition definition,
            Action<SilverlightWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSilverlightWebParts<TModelNode>(this TModelNode model, IEnumerable<SilverlightWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
