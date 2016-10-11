using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class SharePointDesignerSettingsModelNode : TypedModelNode
    {

    }

    public static class SharePointDesignerSettingsSyntax
    {
        #region methods

        public static TModelNode AddSharePointDesignerSettings<TModelNode>(this TModelNode model, SharePointDesignerSettingsDefinition definition)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return AddSharePointDesignerSettings(model, definition, null);
        }

        public static TModelNode AddSharePointDesignerSettings<TModelNode>(this TModelNode model, SharePointDesignerSettingsDefinition definition,
            Action<SharePointDesignerSettingsModelNode> action)
            where TModelNode : ModelNode, ISiteModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload



        #endregion
    }
}
