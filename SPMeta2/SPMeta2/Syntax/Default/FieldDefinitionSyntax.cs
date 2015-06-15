using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class FieldDefinitionSyntax
    {
        #region methods

        public static ModelNode AddField(this ModelNode siteModel, FieldDefinition definition)
        {
            return AddFields(siteModel, new[] { definition });
        }

        public static ModelNode AddField(this ModelNode model, FieldDefinition fielDefinition, Action<ModelNode> action)
        {
            return model.AddDefinitionNode(fielDefinition, action);
        }

        public static ModelNode AddFields(this ModelNode siteModel, FieldDefinition[] fielDefinition)
        {
            return AddFields(siteModel, (IEnumerable<FieldDefinition>)fielDefinition);
        }

        public static ModelNode AddFields(this ModelNode model, IEnumerable<FieldDefinition> fieldDefinitions)
        {
            foreach (var fieldDefinition in fieldDefinitions)
                model.AddDefinitionNode(fieldDefinition);

            return model;
        }

        #endregion


    }
}
