using System;
using System.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using System.IO;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeDefinition); }
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var web = ExtractWeb(modelHost);

            var site = web.Site;
            var contentTypeDefinition = model as ContentTypeDefinition;

            if (site != null && contentTypeDefinition != null)
            {
                var contentTypeId = new SPContentTypeId(contentTypeDefinition.GetContentTypeId());

                // SPBug, it has to be new SPWen for every content type operation inside feature event handler
                using (var tmpRootWeb = site.OpenWeb(web.ID))
                {
                    var targetContentType = tmpRootWeb.ContentTypes[contentTypeId];

                    if (childModelType == typeof(ModuleFileDefinition))
                    {
                        action(new FolderModelHost()
                        {
                            CurrentContentType = targetContentType,
                            CurrentContentTypeFolder = targetContentType.ResourceFolder
                        });
                    }
                    else
                    {
                        action(targetContentType);
                    }

                    targetContentType.Update(true);
                    tmpRootWeb.Update();
                }
            }
            else
            {
                action(modelHost);
            }
        }


        protected SPWeb ExtractWeb(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb;
            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            throw new SPMeta2Exception("modelHost should be SiteModelHost/WebModelHost");
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var web = ExtractWeb(modelHost);
            var contentTypeModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var site = web.Site;
            var targetWeb = web;

            // SPBug, it has to be new SPWen for every content type operation inside feature event handler
            using (var tmpWeb = site.OpenWeb(targetWeb.ID))
            {
                var contentTypeId = new SPContentTypeId(contentTypeModel.GetContentTypeId());

                // by ID, by Name
                var targetContentType = tmpWeb.ContentTypes[contentTypeId];

                if (targetContentType == null)
                {
                    targetContentType = tmpWeb.ContentTypes
                                                  .OfType<SPContentType>()
                                                  .FirstOrDefault(f => f.Name.ToUpper() == contentTypeModel.Name.ToUpper());
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = targetContentType,
                    ObjectType = typeof(SPContentType),
                    ObjectDefinition = contentTypeModel,
                    ModelHost = modelHost
                });

                if (targetContentType == null)
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new content type");

                    targetContentType = tmpWeb
                        .ContentTypes
                        .Add(new SPContentType(contentTypeId, tmpWeb.ContentTypes, contentTypeModel.Name));
                }
                else
                {
                    TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing content type");
                }

                targetContentType.Hidden = contentTypeModel.Hidden;

                targetContentType.Name = contentTypeModel.Name;
                targetContentType.Group = contentTypeModel.Group;

                // SPBug, description cannot be null
                targetContentType.Description = contentTypeModel.Description ?? string.Empty;

                if (!string.IsNullOrEmpty(contentTypeModel.DocumentTemplate))
                {
                    var processedDocumentTemplateUrl = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = contentTypeModel.DocumentTemplate,
                        Context = tmpWeb
                    }).Value;

                    // resource related path
                    if (!processedDocumentTemplateUrl.Contains('/')
                        && !processedDocumentTemplateUrl.Contains('\\'))
                    {
                        processedDocumentTemplateUrl = UrlUtility.CombineUrl(new string[] { 
                            targetContentType.ResourceFolder.ServerRelativeUrl,
                            processedDocumentTemplateUrl
                        });
                    }

                    targetContentType.DocumentTemplate = processedDocumentTemplateUrl;
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioned,
                    Object = targetContentType,
                    ObjectType = typeof(SPContentType),
                    ObjectDefinition = contentTypeModel,
                    ModelHost = modelHost
                });

                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "Calling currentContentType.Update(true)");
                targetContentType.UpdateIncludingSealedAndReadOnly(true);

                tmpWeb.Update();
            }
        }

        #endregion
    }
}
