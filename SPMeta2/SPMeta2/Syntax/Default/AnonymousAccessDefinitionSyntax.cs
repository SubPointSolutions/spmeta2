using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class AnonymousAccessSettingsModelNode : TypedModelNode
    {

    }

    public static class AnonymousAccessSettingsDefinitionSyntax
    {

        #region methods

        public static TModelNode AddAnonymousAccessSettings<TModelNode>(this TModelNode model, AnonymousAccessSettingsDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddAnonymousAccessSettings(model, definition, null);
        }

        public static TModelNode AddAnonymousAccessSettings<TModelNode>(this TModelNode model, AnonymousAccessSettingsDefinition definition,
            Action<AnonymousAccessSettingsModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion


    }
}
