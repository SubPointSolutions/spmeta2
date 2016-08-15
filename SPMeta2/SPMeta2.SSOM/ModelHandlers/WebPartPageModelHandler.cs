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
using SPMeta2.Templates;

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

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;


            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var webpartPageModel = model.WithAssertAndCast<WebPartPageDefinition>("model", value => value.RequireNotNull());

            //var list = listModelHost.HostList;
            var folder = folderModelHost.CurrentLibraryFolder;

            // Web part provision seems to put only the last web part on the page #869
            // https://github.com/SubPointSolutions/spmeta2/issues/869
            var targetFile = FindWebPartPage(folder, webpartPageModel);
            //var targetFile = GetOrCreateNewWebPartFile(modelHost, folder, webpartPageModel);

            using (var webPartManager = targetFile.GetLimitedWebPartManager(PersonalizationScope.Shared))
            {
                var webpartPageHost = new WebpartPageModelHost
                {
                    HostFile = targetFile,
                    PageListItem = targetFile.Item,
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
            var targetPage = GetOrCreateNewWebPartFile(modelHost, folder, webpartPageModel);

            // gosh, it really does not have a title
            //targetPage[SPBuiltInFieldId.Title] = webpartPageModel.Title;



            //targetPage.Update();
        }

        protected SPFile FindWebPartPage(SPFolder folder, WebPartPageDefinition webpartPageModel)
        {
            var webPartPageName = GetSafeWebPartPageFileName(webpartPageModel);

            if (!webPartPageName.EndsWith(".aspx"))
                webPartPageName += ".aspx";

            foreach (SPFile file in folder.Files)
                if (file.Name.ToUpper() == webPartPageName.ToUpper())
                    return file;

            return null;
        }

        protected string GetSafeWebPartPageFileName(WebPartPageDefinition webpartPageModel)
        {
            var webPartPageName = webpartPageModel.FileName;

            if (!webPartPageName.EndsWith(".aspx"))
                webPartPageName += ".aspx";

            return webPartPageName;
        }

        private SPFile GetOrCreateNewWebPartFile(object modelHost, SPFolder folder,
            WebPartPageDefinition definition)
        {
            var list = folder.DocumentLibrary;
            var targetFile = FindWebPartPage(folder, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = targetFile,
                ObjectType = typeof(SPFile),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (targetFile == null || definition.NeedOverride)
            {
                if (definition.NeedOverride)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing web part page");
                    TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "NeedOverride = true. Replacing web part page.");
                }
                else
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new web part page");
                }

                var webPartPageName = GetSafeWebPartPageFileName(definition);

                byte[] fileContent = null;

                if (!string.IsNullOrEmpty(definition.CustomPageLayout))
                {
                    fileContent = Encoding.UTF8.GetBytes(definition.CustomPageLayout);
                }
                else
                {
                    fileContent = Encoding.UTF8.GetBytes(GetWebPartPageTemplateContent(definition));
                }

                ModuleFileModelHandler.DeployModuleFile(folder,
                  SPUrlUtility.CombineUrl(folder.ServerRelativeUrl, webPartPageName),
                  webPartPageName,
                  fileContent,
                  true,
                    file =>
                    {

                    },
                    after =>
                    {
                        FieldLookupService.EnsureDefaultValues(after.ListItemAllFields, definition.DefaultValues);

                        if (!string.IsNullOrEmpty(definition.ContentTypeId) ||
                           !string.IsNullOrEmpty(definition.ContentTypeName))
                        {
                            if (!string.IsNullOrEmpty(definition.ContentTypeId))
                                after.ListItemAllFields["ContentTypeId"] = ContentTypeLookupService.LookupListContentTypeById(list, definition.ContentTypeId);

                            if (!string.IsNullOrEmpty(definition.ContentTypeName))
                                after.ListItemAllFields["ContentTypeId"] = ContentTypeLookupService.LookupContentTypeByName(list, definition.ContentTypeName);
                        }

                        FieldLookupService.EnsureValues(after.ListItemAllFields, definition.Values, true);

                        if (definition.DefaultValues.Any()
                            || definition.Values.Any()
                            || !string.IsNullOrEmpty(definition.ContentTypeId)
                            || !string.IsNullOrEmpty(definition.ContentTypeName))
                        {
                            after.ListItemAllFields.Update();
                        }

                        InvokeOnModelEvent(this, new ModelEventArgs
                        {
                            CurrentModelNode = null,
                            Model = null,
                            EventType = ModelEventType.OnProvisioned,
                            Object = after,
                            ObjectType = typeof(SPFile),
                            ObjectDefinition = definition,
                            ModelHost = modelHost
                        });
                    });

                targetFile = FindWebPartPage(folder, definition);
            }
            else
            {
                FieldLookupService.EnsureDefaultValues(targetFile.ListItemAllFields, definition.DefaultValues);

                if (!string.IsNullOrEmpty(definition.ContentTypeId) ||
                !string.IsNullOrEmpty(definition.ContentTypeName))
                {
                    if (!string.IsNullOrEmpty(definition.ContentTypeId))
                        targetFile.ListItemAllFields["ContentTypeId"] = ContentTypeLookupService.LookupListContentTypeById(list, definition.ContentTypeId);

                    if (!string.IsNullOrEmpty(definition.ContentTypeName))
                        targetFile.ListItemAllFields["ContentTypeId"] = ContentTypeLookupService.LookupContentTypeByName(list, definition.ContentTypeName);
                }

                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing web part page");
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "NeedOverride = false. Skipping replacing web part page.");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = targetFile,
                    ObjectType = typeof(SPFile),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                targetFile.Update();
            }

            return targetFile;
        }


        public virtual string GetWebPartPageTemplateContent(WebPartPageDefinition webPartPageModel)
        {
            // TODO, add support for SP2016
            // Built-in web part page templates should be correctly resolved for SP2010/2013 #683

            var spRuntimeVersion = typeof(SPField).Assembly.GetName().Version;

            if (spRuntimeVersion.Major == 14)
            {
                return GetSP2010WebPartPageTemplateContent(webPartPageModel);
            }

            if (spRuntimeVersion.Major == 15)
            {
                return GetSP2013WebPartPageTemplateContent(webPartPageModel);
            }

            throw new Exception(string.Format("PageLayoutTemplate: [{0}] is not supported.", webPartPageModel.PageLayoutTemplate));
        }

        protected virtual string GetSP2013WebPartPageTemplateContent(WebPartPageDefinition webPartPageModel)
        {
            switch (webPartPageModel.PageLayoutTemplate)
            {
                case 1:
                    return SP2013WebPartPageTemplates.spstd1;
                case 2:
                    return SP2013WebPartPageTemplates.spstd2;
                case 3:
                    return SP2013WebPartPageTemplates.spstd3;
                case 4:
                    return SP2013WebPartPageTemplates.spstd4;
                case 5:
                    return SP2013WebPartPageTemplates.spstd5;
                case 6:
                    return SP2013WebPartPageTemplates.spstd6;
                case 7:
                    return SP2013WebPartPageTemplates.spstd7;
                case 8:
                    return SP2013WebPartPageTemplates.spstd8;
            }

            throw new Exception(string.Format("PageLayoutTemplate: [{0}] is not supported.", webPartPageModel.PageLayoutTemplate));
        }

        protected virtual string GetSP2010WebPartPageTemplateContent(WebPartPageDefinition webPartPageModel)
        {
            switch (webPartPageModel.PageLayoutTemplate)
            {
                case 1:
                    return SP2010WebPartPageTemplates.spstd1;
                case 2:
                    return SP2010WebPartPageTemplates.spstd2;
                case 3:
                    return SP2010WebPartPageTemplates.spstd3;
                case 4:
                    return SP2010WebPartPageTemplates.spstd4;
                case 5:
                    return SP2010WebPartPageTemplates.spstd5;
                case 6:
                    return SP2010WebPartPageTemplates.spstd6;
                case 7:
                    return SP2010WebPartPageTemplates.spstd7;
                case 8:
                    return SP2010WebPartPageTemplates.spstd8;
            }

            throw new Exception(string.Format("PageLayoutTemplate: [{0}] is not supported.", webPartPageModel.PageLayoutTemplate));
        }

        #endregion
    }
}
