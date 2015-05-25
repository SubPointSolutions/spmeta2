using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Standard.Definitions.Webparts;

namespace SPMeta2.Standard.Syntax
{
    public static class TypedWebpartsDefinitionSyntax
    {
        #region ProjectSummaryWebPart

        public static ModelNode AddProjectSummaryWebPart(this ModelNode model, ProjectSummaryWebPartDefinition definition)
        {
            return AddProjectSummaryWebPart(model, definition, null);
        }

        public static ModelNode AddProjectSummaryWebPart(this ModelNode model, ProjectSummaryWebPartDefinition definition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(definition, action);
        }

        public static ModelNode AddProjectSummaryWebParts(this ModelNode model, IEnumerable<ProjectSummaryWebPartDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
