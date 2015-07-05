using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class AudienceModelNode : TypedModelNode
    {

    }

    public static class AudienceDefinitionSyntax
    {
        #region methods

        public static TModelNode AddAudience<TModelNode>(this TModelNode model, AudienceDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddAudience(model, definition, null);
        }

        public static TModelNode AddAudience<TModelNode>(this TModelNode model, AudienceDefinition definition,
            Action<AudienceModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddAudiences<TModelNode>(this TModelNode model, IEnumerable<AudienceDefinition> definitions)
           where TModelNode : ModelNode, ISiteModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
