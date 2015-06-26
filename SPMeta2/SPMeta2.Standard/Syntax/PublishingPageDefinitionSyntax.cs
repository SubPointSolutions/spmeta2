using System;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;
using SPMeta2.Definitions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Standard.Syntax
{
    public class PublishingPageModelNode : TypedModelNode
    {

    }

    public static class DeleteWebPartsDefinitionSyntax
    {
        public static PublishingPageModelNode AddDeleteWebParts(this PublishingPageModelNode model, DeleteWebPartsDefinition definition)
        {
            return AddDeleteWebParts(model, definition, null);
        }

        public static PublishingPageModelNode AddDeleteWebParts(this PublishingPageModelNode model, DeleteWebPartsDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }
    }

    public static class PublishingPageDefinitionSyntax
    {
        #region publishing page

        public static ListModelNode AddPublishingPage(this ListModelNode model, PublishingPageDefinition definition)
        {
            return AddPublishingPage(model, definition, null);
        }

        public static ListModelNode AddPublishingPage(this ListModelNode model, PublishingPageDefinition definition,
            Action<PublishingPageModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static FolderModelNode AddPublishingPage(this FolderModelNode model, PublishingPageDefinition definition)
        {
            return AddPublishingPage(model, definition, null);
        }

        public static FolderModelNode AddPublishingPage(this FolderModelNode model, PublishingPageDefinition definition,
            Action<PublishingPageModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddPublishingPages(this ModelNode model, IEnumerable<PublishingPageDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion

        #region host override

        public static ListModelNode AddHostPublishingPage(this ListModelNode model, PublishingPageDefinition definition)
        {
            return AddHostPublishingPage(model, definition, null);
        }

        public static ListModelNode AddHostPublishingPage(this ListModelNode model,
            PublishingPageDefinition definition, Action<PublishingPageModelNode> action)
        {
            return model.AddTypedDefinitionNodeWithOptions(definition, action, ModelNodeOptions.New().NoSelfProcessing());
        }

        #endregion
    }
}
