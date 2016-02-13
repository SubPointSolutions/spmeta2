using System;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Syntax.Extended
{
    public static class ListDefinitionSyntax
    {
        #region host shortcuts

        public static TModelNode AddHostStyleLibraryList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.StyleLibrary, action);
        }

        public static TModelNode AddHostSitePagesList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.SitePages, action);
        }

        public static TModelNode AddHostDocumentsList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.Documents, action);
        }

        public static TModelNode AddHostSharedDocumentsList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.SharedDocuments, action);
        }

        public static TModelNode AddHostSiteAssetsList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
          where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.SiteAssets, action);
        }

        public static TModelNode AddHostPagesList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
          where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.Pages, action);
        }

        public static TModelNode AddHostComposedLooksList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
         where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.ComposedLooks, action);
        }

        public static TModelNode AddHostDeviceChannelsList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
        where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.DeviceChannels, action);
        }

        public static TModelNode AddHostFormTemplatesList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
        where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.FormTemplates, action);
        }

        public static TModelNode AddHostImagesList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
        where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.Images, action);
        }

        public static TModelNode AddHostListTemplateGalleryList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
        where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.ListTemplateGallery, action);
        }

        public static TModelNode AddHostReusableContentList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
        where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.ReusableContent, action);
        }

        public static TModelNode AddHostSiteCollectionDocumentsList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
        where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.SiteCollectionDocuments, action);
        }

        public static TModelNode AddHostSiteCollectionImagesList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
        where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.SiteCollectionImages, action);
        }

        public static TModelNode AddHostMasterPageList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
             where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.Catalogs.MasterPage, action);
        }

        public static TModelNode AddHostThemeList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.Catalogs.Theme, action);
        }

        public static TModelNode AddHostWebPartCatalogList<TModelNode>(this TModelNode model, Action<ListModelNode> action)
            where TModelNode : ModelNode, IWebModelNode, new()
        {
            return model.AddHostList(BuiltInListDefinitions.Catalogs.Wp, action);
        }

        #endregion
    }
}
