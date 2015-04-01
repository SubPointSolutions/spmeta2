using System;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Enumerations;

namespace SPMeta2.Containers
{
    public static class RandomContainersSyntax
    {
        #region constructors

        static RandomContainersSyntax()
        {
            ModelGeneratorService = new ModelGeneratorService();
        }

        #endregion

        #region properties

        public static ModelGeneratorService ModelGeneratorService { get; set; }

        #endregion

        #region syntax

        #region webs

        public static ModelNode AddRandomWeb(this ModelNode model)
        {
            return AddRandomWebPartPage(model, null);
        }

        public static ModelNode AddRandomWeb(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<WebDefinition>(action);
        }

        #endregion

        #region welcome page

        #region webs

        public static ModelNode AddRandomWelcomePage(this ModelNode model)
        {
            return AddRandomWelcomePage(model, null);
        }

        public static ModelNode AddRandomWelcomePage(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<WelcomePageDefinition>(action);
        }

        #endregion

        #endregion

        #region property bags

        #region webs

        public static ModelNode AddRandomPropertyBag(this ModelNode model)
        {
            return AddRandomPropertyBag(model, null);
        }

        public static ModelNode AddRandomPropertyBag(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<PropertyDefinition>(action);
        }

        #endregion

        #region user custom action

        public static ModelNode AddRandomUserCustomAction(this ModelNode model)
        {
            return AddRandomUserCustomAction(model, null);
        }

        public static ModelNode AddRandomUserCustomAction(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<UserCustomActionDefinition>(action);
        }

        #endregion

        #endregion

        #region webpart pages

        public static ModelNode AddRandomWebPartPage(this ModelNode model)
        {
            return AddRandomWebPartPage(model, null);
        }

        public static ModelNode AddRandomWebPartPage(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<WebPartPageDefinition>(action);
        }

        #endregion

        #region wiki pages

        public static ModelNode AddRandomWikiPage(this ModelNode model)
        {
            return AddRandomWikiPage(model, null);
        }

        public static ModelNode AddRandomWikiPage(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<WikiPageDefinition>(action);
        }

        #endregion

        #region lists

        public static ModelNode AddRandomList(this ModelNode model)
        {
            return AddRandomWebPartPage(model, null);
        }

        public static ModelNode AddRandomList(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<ListDefinition>(action);
        }

        public static ModelNode AddRandomDocumentLibrary(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<ListDefinition>(node =>
            {
                var def = node.Value as ListDefinition;

                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;

                action(node);
            });
        }

        #endregion

        #region audit settings

        public static ModelNode AddRandomAuditSetting(this ModelNode model)
        {
            return AddRandomAuditSetting(model, null);
        }

        public static ModelNode AddRandomAuditSetting(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<AuditSettingsDefinition>(action);
        }

        #endregion

        #region list item



        public static ModelNode AddRandomListItem(this ModelNode model)
        {
            return AddRandomListItem(model, null);
        }

        public static ModelNode AddRandomListItem(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<ListItemDefinition>(action);
        }

        #endregion


        public static ModelNode AddRandomModuleFile(this ModelNode model)
        {
            return AddRandomModuleFile(model, null);
        }

        public static ModelNode AddRandomModuleFile(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<ModuleFileDefinition>(action);
        }

        #region fodlers

        public static ModelNode AddRandomFolder(this ModelNode model)
        {
            return AddRandomFolder(model, null);
        }

        public static ModelNode AddRandomFolder(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<FolderDefinition>(action);
        }

        #endregion


        #region web parts

        public static ModelNode AddRandomWebpart(this ModelNode model)
        {
            return AddRandomWebpart(model, null);
        }

        public static ModelNode AddRandomWebpart(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<WebPartDefinition>(action);
        }

        #endregion

        #region fields

        public static ModelNode AddRandomField(this ModelNode model)
        {
            return AddRandomField(model, null);
        }

        public static ModelNode AddRandomField(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<FieldDefinition>(action);
        }

        #endregion

        #region content types

        public static ModelNode AddRandomContentType(this ModelNode model)
        {
            return AddRandomContentType(model, null);
        }

        public static ModelNode AddRandomContentType(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<ContentTypeDefinition>(action);
        }

        #endregion



        #endregion

        #region internal

        public static ModelNode AddRandomDefinition<TDefinition>(this ModelNode model)
            where TDefinition : DefinitionBase
        {
            return AddRandomDefinition<TDefinition>(model, null);
        }

        public static ModelNode AddRandomDefinition<TDefinition>(this ModelNode model, Action<ModelNode> action)
              where TDefinition : DefinitionBase
        {
            return model.AddDefinitionNode(ModelGeneratorService.GetRandomDefinition<TDefinition>(), action);
        }

        #endregion
    }
}
