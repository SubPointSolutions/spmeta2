using System;
using System.IO;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.Extensions;
using SPMeta2.Utils;
using System.Web.UI.WebControls.WebParts;
using SPMeta2.SSOM.ModelHosts;
using Microsoft.SharePoint.Utilities;
using System.Text;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebPartPageModelHandler : SSOMModelHandlerBase
    {
        #region properties

        private const string WebPartPageCmdTemplate =
                                          "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                          "<Method ID=\"0, NewWebPage\">" +
                                          "<SetList Scope=\"Request\">{0}</SetList>" +
                                          "<SetVar Name=\"ID\">New</SetVar>" +
                                          "<SetVar Name=\"Title\">{1}</SetVar>" +
                                          "<SetVar Name=\"Cmd\">NewWebPage</SetVar>" +
                                          "<SetVar Name=\"WebPartPageTemplate\">{2}</SetVar>" +
                                          "<SetVar Name=\"Type\">WebPartPage</SetVar>" +
                                          "<SetVar Name=\"Overwrite\">{3}</SetVar>" +
                                          "</Method>";

        #endregion

        #region methods

        public override Type TargetType
        {
            get { return typeof(WebPartPageDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var webpartPageModel = model.WithAssertAndCast<WebPartPageDefinition>("model", value => value.RequireNotNull());

            //var list = listModelHost.HostList;
            var folder = folderModelHost.CurrentLibraryFolder;

            var targetPage = GetOrCreateNewWebPartPage(modelHost, folder, webpartPageModel);

            using (var webPartManager = targetPage.File.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                var webpartPageHost = new WebpartPageModelHost
                {
                    HostFile =  targetPage.File,
                    PageListItem =  targetPage,
                    SPLimitedWebPartManager = webPartManager
                };

                action(webpartPageHost);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var webpartPageModel = model.WithAssertAndCast<WebPartPageDefinition>("model", value => value.RequireNotNull());

            //var list = folderModelHost.HostList;
            var folder = folderModelHost.CurrentLibraryFolder;
            var targetPage = GetOrCreateNewWebPartPage(modelHost, folder, webpartPageModel);

            // gosh, it really does not have a title
            //targetPage[SPBuiltInFieldId.Title] = webpartPageModel.Title;



            //targetPage.Update();
        }

        protected SPListItem FindWebPartPage(SPFolder folder, WebPartPageDefinition webpartPageModel)
        {
            var webPartPageName = GetSafeWebPartPageFileName(webpartPageModel);

            if (!webPartPageName.EndsWith(".aspx"))
                webPartPageName += ".aspx";

            foreach (SPFile file in folder.Files)
                if (file.Name.ToUpper() == webPartPageName.ToUpper())
                    return file.Item;

            return null;
        }

        protected string GetSafeWebPartPageFileName(WebPartPageDefinition webpartPageModel)
        {
            var webPartPageName = webpartPageModel.FileName;

            if (!webPartPageName.EndsWith(".aspx"))
                webPartPageName += ".aspx";

            return webPartPageName;
        }

        private SPListItem GetOrCreateNewWebPartPage(object modelHost, SPFolder folder,
            WebPartPageDefinition webpartPageModel)
        {
            var targetPage = FindWebPartPage(folder, webpartPageModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = targetPage == null ? null : targetPage.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = webpartPageModel,
                ModelHost = modelHost
            });

            if (targetPage == null || webpartPageModel.NeedOverride)
            {
                if (webpartPageModel.NeedOverride)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing web part page");
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "NeedOverride = true. Replacing web part page.");
                }
                else
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new web part page");
                }

                var webPartPageName = GetSafeWebPartPageFileName(webpartPageModel);

                byte[] fileContent = null;

                if (!string.IsNullOrEmpty(webpartPageModel.CustomPageLayout))
                {
                    fileContent = Encoding.UTF8.GetBytes(webpartPageModel.CustomPageLayout);
                }
                else
                {
                    fileContent = Encoding.UTF8.GetBytes(GetWebPartTemplateContent(webpartPageModel));
                }

                ModuleFileModelHandler.DeployModuleFile(folder,
                  SPUrlUtility.CombineUrl(folder.ServerRelativeUrl, webPartPageName),
                  webPartPageName,
                  fileContent,
                  true,
                    file =>
                    {
                        InvokeOnModelEvent(this, new ModelEventArgs
                        {
                            CurrentModelNode = null,
                            Model = null,
                            EventType = ModelEventType.OnProvisioned,
                            Object = file,
                            ObjectType = typeof(SPFile),
                            ObjectDefinition = webpartPageModel,
                            ModelHost = modelHost
                        });
                    },
                  null);

                targetPage = FindWebPartPage(folder, webpartPageModel);
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing web part page");
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "NeedOverride = false. Skipping replacing web part page.");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = targetPage == null ? null : targetPage.File,
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = webpartPageModel,
                    ModelHost = modelHost
                });

                targetPage.Update();
            }

            return targetPage;
        }

        protected virtual string GetWebPartTemplateContent(WebPartPageDefinition webPartPageModel)
        {
            return GetWebPartPageTemplateContent(webPartPageModel);
        }

        public static string GetWebPartPageTemplateContent(WebPartPageDefinition webPartPageModel)
        {
            // gosh! would u like to offer a better way?
            switch (webPartPageModel.PageLayoutTemplate)
            {
                case 1:
                    return WebPartPageTemplates.spstd1;
                case 2:
                    return WebPartPageTemplates.spstd2;
                case 3:
                    return WebPartPageTemplates.spstd3;
                case 4:
                    return WebPartPageTemplates.spstd4;
                case 5:
                    return WebPartPageTemplates.spstd5;
                case 6:
                    return WebPartPageTemplates.spstd6;
                case 7:
                    return WebPartPageTemplates.spstd7;
                case 8:
                    return WebPartPageTemplates.spstd8;
            }

            throw new Exception(string.Format("PageLayoutTemplate: [{0}] is not supported.", webPartPageModel.PageLayoutTemplate));
        }

        #endregion
    }
}
