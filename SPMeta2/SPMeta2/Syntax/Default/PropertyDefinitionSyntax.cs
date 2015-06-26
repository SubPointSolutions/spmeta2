using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public class PropertyModelNode : TypedModelNode
    {

    }

    public static class PropertyDefinitionSyntax
    {

        public static FarmModelNode AddProperty(this FarmModelNode model, PropertyDefinition definition)
        {
            return AddProperty(model, definition, null);
        }

        public static FarmModelNode AddProperty(this FarmModelNode model, PropertyDefinition definition, Action<PropertyModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebApplicationModelNode AddProperty(this WebApplicationModelNode model, PropertyDefinition definition)
        {
            return AddProperty(model, definition, null);
        }

        public static WebApplicationModelNode AddProperty(this WebApplicationModelNode model, PropertyDefinition definition, Action<PropertyModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static SiteModelNode AddProperty(this SiteModelNode model, PropertyDefinition definition)
        {
            return AddProperty(model, definition, null);
        }

        public static SiteModelNode AddProperty(this SiteModelNode model, PropertyDefinition definition, Action<PropertyModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static WebModelNode AddProperty(this WebModelNode model, PropertyDefinition definition)
        {
            return AddProperty(model, definition, null);
        }

        public static WebModelNode AddProperty(this WebModelNode model, PropertyDefinition definition, Action<PropertyModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static FolderModelNode AddProperty(this FolderModelNode model, PropertyDefinition definition)
        {
            return AddProperty(model, definition, null);
        }

        public static FolderModelNode AddProperty(this FolderModelNode model, PropertyDefinition definition, Action<PropertyModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ListItemModelNode AddProperty(this ListItemModelNode model, PropertyDefinition definition)
        {
            return AddProperty(model, definition, null);
        }

        public static ListItemModelNode AddProperty(this ListItemModelNode model, PropertyDefinition definition, Action<PropertyModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }


        #region array overload

        public static FarmModelNode AddProperties(this FarmModelNode model, IEnumerable<PropertyDefinition> definitions)
        {
            return AddPropertiesInternal(model, definitions);
        }

        public static WebApplicationModelNode AddProperties(this WebApplicationModelNode model, IEnumerable<PropertyDefinition> definitions)
        {
            return AddPropertiesInternal(model, definitions);
        }

        public static SiteModelNode AddProperties(this SiteModelNode model, IEnumerable<PropertyDefinition> definitions)
        {
            return AddPropertiesInternal(model, definitions);
        }

        public static WebModelNode AddProperties(this WebModelNode model, IEnumerable<PropertyDefinition> definitions)
        {
            return AddPropertiesInternal(model, definitions);
        }

        #endregion

        #region utils

        private static T AddPropertiesInternal<T>(T model, IEnumerable<PropertyDefinition> definitions)
            where T : ModelNode
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}