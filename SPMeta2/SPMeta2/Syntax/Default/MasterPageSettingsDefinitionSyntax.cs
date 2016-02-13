using System;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class MasterPageSettingsModelNode : TypedModelNode
    {

    }

    public static class MasterPageSettingsDefinitionSyntax
    {
        #region methods

        public static TModelNode AddMasterPageSettings<TModelNode>(this TModelNode model, MasterPageSettingsDefinition definition)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return AddMasterPageSettings(model, definition, null);
        }

        public static TModelNode AddMasterPageSettings<TModelNode>(this TModelNode model, MasterPageSettingsDefinition definition,
            Action<MasterPageSettingsModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
