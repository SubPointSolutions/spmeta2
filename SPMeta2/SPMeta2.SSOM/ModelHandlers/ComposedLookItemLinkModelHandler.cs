using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ComposedLookItemLinkModelHandler : SSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ComposedLookItemLinkDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var typedModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<ComposedLookItemLinkDefinition>("model", value => value.RequireNotNull());

            ApplyComposedLookUnderWeb(modelHost, typedModelHost, typedDefinition);
        }

        private void ApplyComposedLookUnderWeb(object modelHost, WebModelHost typedModelHost,
            ComposedLookItemLinkDefinition typedDefinition)
        {
            var web = typedModelHost.HostWeb;
            var rootWeb = typedModelHost.HostWeb.Site.RootWeb;

            var siteRelativeUrl = web.Site.ServerRelativeUrl;

            var composedLookName = typedDefinition.ComposedLookItemName;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            var composedLookList = web.GetCatalog(SPListTemplateType.DesignCatalog);
            var items = composedLookList.GetItems(new SPQuery
            {
                Query = CamlQueryUtils.WhereItemByFieldValueQuery("Name", composedLookName),
                RowLimit = 1
            });
            var targetComposedLookItem = items.Count > 0 ? items[0] : null;

            if (targetComposedLookItem == null)
                throw new SPMeta2Exception(string.Format("Can't find composed look by name: [{0}]", composedLookName));

            var name = ConvertUtils.ToString(targetComposedLookItem["Name"]);

            var masterPageUrl = GetUrlValue(targetComposedLookItem, "MasterPageUrl");

            if (!string.IsNullOrEmpty(masterPageUrl))
            {
                var targetMasterPageUrl = masterPageUrl;

                if (SPUrlUtility.IsUrlFull(masterPageUrl))
                    targetMasterPageUrl = web.GetServerRelativeUrlFromUrl(masterPageUrl);

                web.CustomMasterUrl = targetMasterPageUrl;
                web.MasterUrl = targetMasterPageUrl;

                web.Update();
            }

            var themeUrl = GetUrlValue(targetComposedLookItem, "ThemeUrl");
            var fontSchemeUrl = GetUrlValue(targetComposedLookItem, "FontSchemeUrl");
            var imageUrl = GetUrlValue(targetComposedLookItem, "ImageUrl");

            var themeFile = rootWeb.GetFile(themeUrl);
            SPFile fontFile = null;

            if (!string.IsNullOrEmpty(fontSchemeUrl))
                fontFile = rootWeb.GetFile(fontSchemeUrl);

            SPTheme theme = null;

            if (!string.IsNullOrEmpty(imageUrl))
            {
                theme = SPTheme.Open(name,
                    themeFile,
                    fontFile != null && fontFile.Exists ? fontFile : null,
                    new Uri(imageUrl));
            }
            else
            {
                theme = SPTheme.Open(name,
                    themeFile,
                    fontFile != null && fontFile.Exists ? fontFile : null);
            }

            theme.ApplyTo(web, false);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(SPWeb),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            // cirrent web was updated by theme engine
            // http://ehikioya.com/apply-themes-to-sharepoint/ 
            typedModelHost.ShouldUpdateHost = false;
        }

        private string GetUrlValue(SPListItem item, string filedName)
        {
            var urlValue = ConvertUtils.ToString(item[filedName]);
            return new SPFieldUrlValue(urlValue).Url;
        }


    }
}
