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
    public class MetadataNavigationSettingsModelNode : TypedModelNode
    {

    }

    public static class MetadataNavigationSettingsSyntax
    {
        #region publishing page

        #region methods

        public static TModelNode AddMetadataNavigationSettings<TModelNode>(this TModelNode model,
            MetadataNavigationSettingsDefinition definition)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return AddMetadataNavigationSettings(model, definition, null);
        }

        public static TModelNode AddMetadataNavigationSettings<TModelNode>(this TModelNode model,
            MetadataNavigationSettingsDefinition definition,
            Action<TModelNode> action)
            where TModelNode : ModelNode, IListModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload


        #endregion

        #endregion
    }
}
