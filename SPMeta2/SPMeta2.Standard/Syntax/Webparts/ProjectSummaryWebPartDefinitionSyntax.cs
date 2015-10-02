using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Standard.Syntax
{

    [Serializable]
    [DataContract]
    public class ProjectSummaryWebPartModelNode : WebPartModelNode
    {

    }

    public static class ProjectSummaryWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddProjectSummaryWebPart<TModelNode>(this TModelNode model, ProjectSummaryWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddProjectSummaryWebPart(model, definition, null);
        }

        public static TModelNode AddProjectSummaryWebPart<TModelNode>(this TModelNode model, ProjectSummaryWebPartDefinition definition,
            Action<ProjectSummaryWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddProjectSummaryWebParts<TModelNode>(this TModelNode model, IEnumerable<ProjectSummaryWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
