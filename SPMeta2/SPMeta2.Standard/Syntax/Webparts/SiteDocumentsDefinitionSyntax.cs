using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Standard.Syntax
{

    [Serializable]
    [DataContract]
    public class SiteDocumentsModelNode : WebPartModelNode
    {

    }

    public static class SiteDocumentsDefinitionSyntax
    {
        #region methods

        public static TModelNode AddSiteDocuments<TModelNode>(this TModelNode model, SiteDocumentsDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddSiteDocuments(model, definition, null);
        }

        public static TModelNode AddSiteDocuments<TModelNode>(this TModelNode model, SiteDocumentsDefinition definition,
            Action<SiteDocumentsModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddSiteDocuments<TModelNode>(this TModelNode model, IEnumerable<SiteDocumentsDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
