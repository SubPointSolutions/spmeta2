using System;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class FieldModelNode : TypedModelNode
    {
        
    }


    public static class FieldDefinitionSyntax
    {
        #region methods

        public static SiteModelNode AddField(this SiteModelNode siteModel, FieldDefinition definition)
        {
            return AddField(siteModel, null);
        }

        public static SiteModelNode AddField(this SiteModelNode model, FieldDefinition definition,
            Action<FieldModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddField(this WebModelNode siteModel, FieldDefinition definition)
        {
            return AddField(siteModel, null);
        }

        public static WebModelNode AddField(this WebModelNode model, FieldDefinition definition, Action<FieldModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ListModelNode AddField(this ListModelNode siteModel, FieldDefinition definition)
        {
            return AddField(siteModel, null);
        }

        public static ListModelNode AddField(this ListModelNode model, FieldDefinition definition, Action<FieldModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static SiteModelNode AddFields(this SiteModelNode model, IEnumerable<FieldDefinition> fieldDefinitions)
        {
            foreach (var fieldDefinition in fieldDefinitions)
                model.AddDefinitionNode(fieldDefinition);

            return model;
        }

        public static WebModelNode AddFields(this WebModelNode model, IEnumerable<FieldDefinition> fieldDefinitions)
        {
            foreach (var fieldDefinition in fieldDefinitions)
                model.AddDefinitionNode(fieldDefinition);

            return model;
        }

        public static ListModelNode AddFields(this ListModelNode model, IEnumerable<FieldDefinition> fieldDefinitions)
        {
            foreach (var fieldDefinition in fieldDefinitions)
                model.AddDefinitionNode(fieldDefinition);

            return model;
        }

        #endregion


    }
}
