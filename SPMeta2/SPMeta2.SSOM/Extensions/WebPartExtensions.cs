using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.Extensions
{
    public static class WebPartExtensions
    {
        public static System.Web.UI.WebControls.WebParts.WebPart ResolveWebPartInstance(SPSite site,
           SPLimitedWebPartManager webPartManager,
           WebPartDefinition webpartModel)
        {
            System.Web.UI.WebControls.WebParts.WebPart webpartInstance = null;

            if (!string.IsNullOrEmpty(webpartModel.WebpartType))
            {
                var webPartType = Type.GetType(webpartModel.WebpartType);
                webpartInstance = Activator.CreateInstance(webPartType) as System.Web.UI.WebControls.WebParts.WebPart;
            }
            else if (!string.IsNullOrEmpty(webpartModel.WebpartFileName))
            {
                var webpartFileName = webpartModel.WebpartFileName;
                var rootWeb = site.RootWeb;

                // load definition from WP catalog
                var webpartCatalog = rootWeb.GetCatalog(SPListTemplateType.WebPartCatalog);
                var webpartItem = webpartCatalog.Items.OfType<SPListItem>().FirstOrDefault(
                        i => string.Compare(i.Name, webpartFileName, true) == 0);

                if (webpartItem == null)
                    throw new ArgumentException(string.Format("webpartItem. Can't find web part file with name: {0}", webpartFileName));

                using (var streamReader = new MemoryStream(webpartItem.File.OpenBinary()))
                {
                    using (var xmlReader = XmlReader.Create(streamReader))
                    {
                        var errMessage = string.Empty;
                        webpartInstance = webPartManager.ImportWebPart(xmlReader, out errMessage);

                        if (!string.IsNullOrEmpty(errMessage))
                            throw new ArgumentException(
                                string.Format("Can't import web part foe with name: {0}. Error: {1}", webpartFileName, errMessage));
                    }
                }
            }
            else if (!string.IsNullOrEmpty(webpartModel.WebpartXmlTemplate))
            {
                var stringBytes = Encoding.UTF8.GetBytes(webpartModel.WebpartXmlTemplate);

                using (var streamReader = new MemoryStream(stringBytes))
                {
                    using (var xmlReader = XmlReader.Create(streamReader))
                    {
                        var errMessage = string.Empty;
                        webpartInstance = webPartManager.ImportWebPart(xmlReader, out errMessage);

                        if (!string.IsNullOrEmpty(errMessage))
                            throw new ArgumentException(
                                string.Format("Can't import web part for XML template: {0}. Error: {1}",
                                webpartModel.WebpartXmlTemplate, errMessage));
                    }
                }
            }
            else
            {
                throw new Exception("Either WebpartType or WebpartFileName or WebpartXmlTemplate needs to be defined.");
            }

            return webpartInstance;
        }

        public static void DeployWebPartsToPage(SPLimitedWebPartManager webPartManager,
            WebPartDefinition webpartDefinitions)
        {
            DeployWebPartToPage(webPartManager, webpartDefinitions, null, null);
        }

        public static void WithExistingWebPart(SPListItem targetPage,
            WebPartDefinition definition,
            Action<SPLimitedWebPartManager, WebPart> webpart)
        {
            WithLimitedWebPartManager(targetPage, webPartManager =>
            {
                var webParts = webPartManager.WebParts.OfType<WebPart>();

                // by ID
                var currentWebPart = webParts.FirstOrDefault(wp =>
                                                !string.IsNullOrEmpty(wp.ID) &&
                                                wp.ID.ToUpper() == definition.Id.ToUpper());

                // by type
                if (currentWebPart == null)
                {
                    var webPartInstance = ResolveWebPartInstance(webPartManager.Web.Site, webPartManager, definition);
                    var webPartInstanceType = webPartInstance.GetType();

                    currentWebPart = webParts.FirstOrDefault(wp => wp.GetType() == webPartInstanceType);
                }

                if (currentWebPart != null)
                    webpart(webPartManager, currentWebPart);
                else
                {
                    throw new SPMeta2Exception(string.Format("Cannot lookup web part by definition:[{0}]", definition));
                }
            });

        }

        public static void DeployWebPartToPage(SPLimitedWebPartManager webPartManager,
            WebPartDefinition webpartDefinitions,
            Action<WebPart> onUpdating,
            Action<WebPart> onUpdated)
        {
            DeployWebPartToPage(webPartManager, webpartDefinitions, onUpdating, onUpdated, null);
        }

        public static void DeployWebPartToPage(SPLimitedWebPartManager webPartManager,
            WebPartDefinition webpartModel,
            Action<WebPart> onUpdating,
            Action<WebPart> onUpdated,
            Action<WebPart, WebPartDefinition> onProcessCommonProperties)
        {
            var site = webPartManager.Web.Site;
            var webPartInstance = ResolveWebPartInstance(site, webPartManager, webpartModel);

            if (onUpdating != null)
                onUpdating(webPartInstance);

            // webpartModel.InvokeOnModelUpdatingEvents<WebPartDefinition, AspWebPart.WebPart>(webPartInstance);

            var needUpdate = false;
            var targetWebpartType = webPartInstance.GetType();

            foreach (System.Web.UI.WebControls.WebParts.WebPart existingWebpart in webPartManager.WebParts)
            {
                if (existingWebpart.ID == webpartModel.Id && existingWebpart.GetType() == targetWebpartType)
                {
                    webPartInstance = existingWebpart;
                    needUpdate = true;
                    break;
                }
            }

            // process common properties
            webPartInstance.Title = webpartModel.Title;
            webPartInstance.ID = webpartModel.Id;

            if (onProcessCommonProperties != null)
                onProcessCommonProperties(webPartInstance, webpartModel);

            // faking context for CQWP deployment
            var webDeploymentAction = new Action(delegate()
            {
                // webpartModel.InvokeOnModelUpdatedEvents<WebPartDefinition, AspWebPart.WebPart>(webPartInstance);

                if (!needUpdate)
                {
                    if (onUpdating != null)
                        onUpdating(webPartInstance);

                    if (onUpdated != null)
                        onUpdated(webPartInstance);

                    webPartManager.AddWebPart(webPartInstance, webpartModel.ZoneId, webpartModel.ZoneIndex);
                }
                else
                {
                    if (webPartInstance.ZoneIndex != webpartModel.ZoneIndex)
                        webPartManager.MoveWebPart(webPartInstance, webpartModel.ZoneId, webpartModel.ZoneIndex);

                    if (onUpdating != null)
                        onUpdating(webPartInstance);

                    if (onUpdated != null)
                        onUpdated(webPartInstance);

                    webPartManager.SaveChanges(webPartInstance);
                }
            });

            if (SPContext.Current == null)
                SPContextExtensions.WithFakeSPContextScope(webPartManager.Web, webDeploymentAction);
            else
                webDeploymentAction();
        }

        public static void WithLimitedWebPartManager(SPListItem targetPage, Action<SPLimitedWebPartManager> action)
        {
            using (var webPartManager = targetPage.File.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                action(webPartManager);
            }
        }

        public static void DeployWebPartToPage(SPListItem targetPage, WebPartDefinition webpartDefinitions)
        {
            var webPartModels = webpartDefinitions;

            WithLimitedWebPartManager(targetPage, webPartManager =>
            {
                DeployWebPartsToPage(webPartManager, webpartDefinitions);
            });
        }
    }
}
