using System;
using System.IO;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Utils;
using System.Collections.Generic;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ComposedLookItemLinkModelHandler : ListItemModelHandler
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

        private void ApplyComposedLookUnderWeb(object modelHost, WebModelHost typedModelHost, ComposedLookItemLinkDefinition typedDefinition)
        {
            var web = typedModelHost.HostWeb;
            var rootWeb = typedModelHost.HostSite.RootWeb;

            var context = web.Context;
            var serverRelatieSiteUrl = typedModelHost.HostSite.ServerRelativeUrl;

            var composedLookName = typedDefinition.ComposedLookItemName;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            var composedLookList = web.GetCatalog((int)ListTemplateType.DesignCatalog);
            var items = composedLookList.GetItems(CamlQueryTemplates.ItemByFieldValueQuery("Name", composedLookName));

            context.Load(items);
            context.ExecuteQueryWithTrace();

            var targetComposedLookItem = items.Count > 0 ? items[0] : null;

            if (targetComposedLookItem == null)
                throw new SPMeta2Exception(string.Format("Can't find composed look by name: [{0}]", composedLookName));

            var name = ConvertUtils.ToString(targetComposedLookItem["Name"]);

            var masterPageUrl = GetUrlValue(targetComposedLookItem, "MasterPageUrl");

            if (!string.IsNullOrEmpty(masterPageUrl))
            {
                // server relative fun
                var targetMasterPageUrl = UrlUtility.CombineUrl(new[]
                {
                    web.ServerRelativeUrl,
                    "_catalogs/masterpage",
                    Path.GetFileName(masterPageUrl)
                });

                web.CustomMasterUrl = targetMasterPageUrl;
                web.MasterUrl = targetMasterPageUrl;

                web.Update();
                context.ExecuteQueryWithTrace();
            }

            var themeUrl = GetUrlValue(targetComposedLookItem, "ThemeUrl");
            var fontSchemeUrl = GetUrlValue(targetComposedLookItem, "FontSchemeUrl");
            var imageUrl = GetUrlValue(targetComposedLookItem, "ImageUrl");

            themeUrl = GetServerRelativeUrlFromFullUrl(serverRelatieSiteUrl, themeUrl);
            fontSchemeUrl = GetServerRelativeUrlFromFullUrl(serverRelatieSiteUrl, fontSchemeUrl);
            imageUrl = GetServerRelativeUrlFromFullUrl(serverRelatieSiteUrl, imageUrl);

            web.ApplyTheme(themeUrl,
                          !string.IsNullOrEmpty(fontSchemeUrl) ? fontSchemeUrl : null,
                          !string.IsNullOrEmpty(imageUrl) ? imageUrl : null,
                          false);

            context.ExecuteQueryWithTrace();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = web,
                ObjectType = typeof(Web),
                ObjectDefinition = typedDefinition,
                ModelHost = modelHost
            });

            // cirrent web was updated by theme engine
            // http://ehikioya.com/apply-themes-to-sharepoint/ 
            typedModelHost.ShouldUpdateHost = false;
        }

        private string GetServerRelativeUrlFromFullUrl(string siteServerRelativeUrl, string fullUrl)
        {
            if (!fullUrl.Contains("://"))
                return fullUrl;

            var safeFullUrlParts = fullUrl.Split(new string[] { "://" }, StringSplitOptions.None);
            var safeFullUrl = safeFullUrlParts.Count() > 1 ? safeFullUrlParts[1] : safeFullUrlParts[0];

            safeFullUrl = safeFullUrl.Replace("//", "/");

            var fullBits = safeFullUrl.Split(new[] { siteServerRelativeUrl }, StringSplitOptions.None).ToList();
            if (fullBits.Any())
                fullBits.RemoveAt(0);

            var resultArray = new List<string>();
            resultArray.Add(siteServerRelativeUrl);
            resultArray.AddRange(fullBits);

            var result = UrlUtility.CombineUrl(resultArray);

            return result;

            //   "http://cloud9:31415/_catalogs/theme/15/palette002.spcolor".Split(//'")[1]
        }

        private string GetUrlValue(ListItem item, string filedName)
        {
            if (item[filedName] != null)
            {
                return (item[filedName] as FieldUrlValue).Url;
            }

            return string.Empty;
        }
    }
}
