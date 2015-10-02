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
    public class SiteModelNode : TypedModelNode, ISiteModelNode,
        IFieldHostModelNode,
        IContentTypeHostModelNode,
        IPropertyHostModelNode,
        IEventReceiverHostModelNode,
        IWebHostModelNode,
        IManagedPropertyHostModelNode,
        IAuditSettingsHostModelNode,
        IFeatureHostModelNode,
        IUserCustomActionHostModelNode,
        ITaxonomyTermStoreHostModelNode,
        ISearchSettingsHostModelNode,
        ISecurityGroupHostModelNode
    {

    }

    public static class SiteDefinitionSyntax
    {
        #region add host

        public static TModelNode AddHostSite<TModelNode>(this TModelNode model, SiteDefinition definition)
             where TModelNode : ModelNode, ISiteHostModelNode, new()
        {
            return AddHostSite(model, definition, null);
        }
        public static TModelNode AddHostSite<TModelNode>(this TModelNode model, SiteDefinition definition,
            Action<SiteModelNode> action)
            where TModelNode : ModelNode, ISiteHostModelNode, new()
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion

        #region methods

        public static TModelNode AddSite<TModelNode>(this TModelNode model, SiteDefinition definition)
            where TModelNode : ModelNode, ISiteHostModelNode, new()
        {
            return AddSite(model, definition, null);
        }

        public static TModelNode AddSite<TModelNode>(this TModelNode model, SiteDefinition definition,
            Action<SiteModelNode> action)
            where TModelNode : ModelNode, ISiteHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSites<TModelNode>(this TModelNode model, IEnumerable<SiteDefinition> definitions)
           where TModelNode : ModelNode, ISiteHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
