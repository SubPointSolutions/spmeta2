using System;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default.Extensions;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

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

        #region apps

        public static WebModelNode AddRandomApp(this WebModelNode model)
        {
            return AddRandomApp(model, null);
        }

        public static WebModelNode AddRandomApp(this WebModelNode model, Action<WebModelNode> action)
        {
            return model.AddRandomTypedDefinition<AppDefinition, WebModelNode, WebModelNode>(action);
        }

        #endregion

        #region webs

        public static WebModelNode AddRandomWeb(this WebModelNode model)
        {
            return AddRandomWeb(model, null);
        }

        public static WebModelNode AddRandomWeb(this WebModelNode model, Action<WebModelNode> action)
        {
            return model.AddRandomTypedDefinition<WebDefinition, WebModelNode, WebModelNode>(action);
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

        #region security groups

        public static ModelNode AddRandomSecurityGroup(this ModelNode model)
        {
            return AddRandomSecurityGroup(model, null);
        }

        public static ModelNode AddRandomSecurityGroup(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<SecurityGroupDefinition>(action);
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

        public static TModelNode AddRandomWebPartPage<TModelNode>(this TModelNode model)
            where TModelNode : TypedModelNode, IListItemHostModelNode, new()
        {
            return AddRandomWebPartPage(model, null);
        }

        public static TModelNode AddRandomWebPartPage<TModelNode>(this TModelNode model, Action<WebPartPageModelNode> action)
            where TModelNode : TypedModelNode, IListItemHostModelNode, new()
        {
            return model.AddRandomTypedDefinition<WebPartPageDefinition, TModelNode, WebPartPageModelNode>(action);
        }

        #endregion

        #region wiki pages

        //public static TModelNode AddRandomWebPartPage<TModelNode>(this TModelNode model)
        //     where TModelNode : TypedModelNode, IListItemHostModelNode, new()
        //{
        //    return AddRandomWebPartPage(model, null);
        //}

        //public static TModelNode AddRandomWebPartPage<TModelNode>(this TModelNode model,
        //    Action<WikiPageModelNode> action)
        //       where TModelNode : TypedModelNode, IListItemHostModelNode, new()
        //{
        //    return model.AddRandomTypedDefinition<WebPartPageDefinition, TModelNode, WikiPageModelNode>(action);
        //}

        public static TModelNode AddRandomWikiPage<TModelNode>(this TModelNode model)
               where TModelNode : TypedModelNode, IListItemHostModelNode, new()
        {
            return AddRandomWikiPage(model, null);
        }

        public static TModelNode AddRandomWikiPage<TModelNode>(this TModelNode model,
            Action<WikiPageModelNode> action)
               where TModelNode : TypedModelNode, IListItemHostModelNode, new()
        {
            return model.AddRandomTypedDefinition<WikiPageDefinition, TModelNode, WikiPageModelNode>(action);
        }

        #endregion

        #region lists

        public static WebModelNode AddRandomList(this WebModelNode model)
        {
            return AddRandomList(model, null);
        }

        public static WebModelNode AddRandomList(this WebModelNode model, Action<ListModelNode> action)
        {
            return model.AddRandomTypedDefinition<ListDefinition, WebModelNode, ListModelNode>(action);
        }

        public static WebModelNode AddRandomDocumentLibrary(this WebModelNode model)
        {
            return AddRandomDocumentLibrary(model, null);
        }

        public static WebModelNode AddRandomDocumentLibrary(this WebModelNode model, Action<ListModelNode> action)
        {
            return model.AddRandomTypedDefinition<ListDefinition, WebModelNode, ListModelNode>(node =>
            {
                var def = node.Value as ListDefinition;

                def.ContentTypesEnabled = true;
                def.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;

                if (action != null)
                    action(node);
            });
        }

        #endregion

        #region list views

        public static ListModelNode AddRandomListView(this ListModelNode model)
        {
            return AddRandomListView(model, null);
        }

        public static ListModelNode AddRandomListView(this ListModelNode model, Action<ListViewModelNode> action)
        {
            return model.AddRandomTypedDefinition<ListViewDefinition, ListModelNode, ListViewModelNode>(action);
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



        public static FolderModelNode AddRandomListItem(this FolderModelNode model)
        {
            return AddRandomListItem(model, null);
        }

        public static FolderModelNode AddRandomListItem(this FolderModelNode model,
            Action<ListItemModelNode> action)
        {
            return model.AddRandomTypedDefinition<ListItemDefinition, FolderModelNode, ListItemModelNode>(action);
        }

        public static ListModelNode AddRandomListItem(this ListModelNode model)
        {
            return AddRandomListItem(model, null);
        }

        public static ListModelNode AddRandomListItem(this ListModelNode model,
            Action<ListItemModelNode> action)
        {
            return model.AddRandomTypedDefinition<ListItemDefinition, ListModelNode, ListItemModelNode>(action);
        }



        #endregion


        public static TModelNode AddRandomModuleFile<TModelNode>(this TModelNode model)
            where TModelNode : TypedModelNode, IModuleFileHostModelNode, new()
        {
            return AddRandomModuleFile(model, null);
        }

        public static TModelNode AddRandomModuleFile<TModelNode>(this TModelNode model, Action<ModuleFileModelNode> action)
           where TModelNode : TypedModelNode, IModuleFileHostModelNode, new()
        {
            //return model.AddRandomDefinition<ModuleFileDefinition>(action);
            return model.AddRandomTypedDefinition<ModuleFileDefinition, TModelNode, ModuleFileModelNode>(action);
        }

        #region fodlers

        public static ListModelNode AddRandomFolder(this ListModelNode model)
        {
            return AddRandomFolder(model, null);
        }

        public static ListModelNode AddRandomFolder(this ListModelNode model, Action<FolderModelNode> action)
        {
            return model.AddRandomTypedDefinition<FolderDefinition, ListModelNode, FolderModelNode>(action);
        }

        public static FolderModelNode AddRandomFolder(this FolderModelNode model)
        {
            return AddRandomFolder(model, null);
        }

        public static FolderModelNode AddRandomFolder(this FolderModelNode model, Action<FolderModelNode> action)
        {
            return model.AddRandomTypedDefinition<FolderDefinition, FolderModelNode, FolderModelNode>(action);
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

        public static ModelNode AddRandomClientWebpart(this ModelNode model)
        {
            return AddRandomClientWebpart(model, null);
        }

        public static ModelNode AddRandomClientWebpart(this ModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomDefinition<ClientWebPartDefinition>(action);
        }

        #endregion

        #region fields

        public static SiteModelNode AddRandomField(this SiteModelNode model)
        {
            return AddRandomField(model, null);
        }

        public static SiteModelNode AddRandomField(this SiteModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomTypedDefinition<FieldDefinition, SiteModelNode, FieldModelNode>(action);
        }

        public static WebModelNode AddRandomField(this WebModelNode model)
        {
            return AddRandomField(model, null);
        }

        public static WebModelNode AddRandomField(this WebModelNode model, Action<ModelNode> action)
        {
            return model.AddRandomTypedDefinition<FieldDefinition, WebModelNode, FieldModelNode>(action);
        }

        #endregion

        #region content types

        public static SiteModelNode AddRandomContentType(this SiteModelNode model)
        {
            return AddRandomContentType(model, null);
        }

        public static SiteModelNode AddRandomContentType(this SiteModelNode model,
            Action<ContentTypeModelNode> action)
        {
            return model.AddRandomTypedDefinition<ContentTypeDefinition, SiteModelNode, ContentTypeModelNode>(action);
        }

        public static WebModelNode AddRandomContentType(this WebModelNode model)
        {
            return AddRandomContentType(model, null);
        }

        public static WebModelNode AddRandomContentType(this WebModelNode model,
            Action<ContentTypeModelNode> action)
        {
            return model.AddRandomTypedDefinition<ContentTypeDefinition, WebModelNode, ContentTypeModelNode>(action);
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

        public static TModelNode AddRandomTypedDefinition<TDefinition, TModelNode, TDefinitionNode>(
           this TModelNode model)
            where TDefinition : DefinitionBase
            where TModelNode : TypedModelNode, new()
            where TDefinitionNode : TypedModelNode, new()
        {
            return AddRandomTypedDefinition<TDefinition, TModelNode, TDefinitionNode>(model, null);
        }

        public static TModelNode AddRandomTypedDefinition<TDefinition, TModelNode, TDefinitionNode>(
            this TModelNode model, Action<TDefinitionNode> action)
            where TDefinition : DefinitionBase
            where TModelNode : TypedModelNode, new()
            where TDefinitionNode : TypedModelNode, new()
        {
            return model.AddTypedDefinitionNode(ModelGeneratorService.GetRandomDefinition<TDefinition>(), action);
        }

        #endregion
    }
}
