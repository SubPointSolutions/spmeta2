using System;
using System.Linq;

using Microsoft.SharePoint;

using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.ModelHosts;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeModelHandler : SSOMModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeDefinition); }
        }

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var web = ExtractWeb(modelHost);

            var site = web.Site;
            var contentTypeDefinition = model as ContentTypeDefinition;

            if (site != null && contentTypeDefinition != null)
            {
                var contentTypeId = new SPContentTypeId(contentTypeDefinition.GetContentTypeId());

                // SPBug, it has to be new SPWeb for every content type operation inside feature event handler
                using (var tmpRootWeb = site.OpenWeb(web.ID))
                {
                    var targetContentType = tmpRootWeb.ContentTypes[contentTypeId];

                    if (childModelType == typeof(ModuleFileDefinition))
                    {
                        action(new FolderModelHost
                        {
                            CurrentContentType = targetContentType,
                            CurrentContentTypeFolder = targetContentType.ResourceFolder
                        });
                    }
                    else
                    {
                        action(ModelHostBase.Inherit<ContentTypeModelHost>(modelHost as ModelHostBase, host =>
                        {
                            host.HostContentType = targetContentType;
                        }));
                    }

                    if (!targetContentType.ReadOnly)
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
                if (string.IsNullOrEmpty(contentTypeModel.ParentContentTypeId))
                {
                    var parentContentType = web.AvailableContentTypes
                                               .OfType<SPContentType>()
                                               .FirstOrDefault(ct => String.Equals(ct.Name, contentTypeModel.ParentContentTypeName, StringComparison.CurrentCultureIgnoreCase));

                    if (parentContentType == null)
                        throw new SPMeta2Exception(string.Format("Cannot find parent content type by giving name: [{0}]", contentTypeModel.ParentContentTypeName));

                    contentTypeModel.ParentContentTypeId = parentContentType.Id.ToString();
                }

                var contentTypeId = new SPContentTypeId(contentTypeModel.GetContentTypeId());

                // by ID, by Name
                var targetContentType = tmpWeb.ContentTypes[contentTypeId];

                // fallback to name
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

                if (contentTypeModel.Sealed.HasValue)
                    targetContentType.Sealed = contentTypeModel.Sealed.Value;

                if (contentTypeModel.ReadOnly.HasValue)
                    targetContentType.ReadOnly = contentTypeModel.ReadOnly.Value;

                // SPBug, description cannot be null
                targetContentType.Description = contentTypeModel.Description ?? string.Empty;

#if !NET35
                if (!string.IsNullOrEmpty(contentTypeModel.JSLink))
                    targetContentType.JSLink = contentTypeModel.JSLink;
#endif

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

                ProcessFormProperties(targetContentType, contentTypeModel);
                ProcessLocalization(targetContentType, contentTypeModel);

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

                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "Calling currentContentType.UpdateIncludingSealedAndReadOnly(true)");
                targetContentType.UpdateIncludingSealedAndReadOnly(true);

                tmpWeb.Update();
            }
        }

        protected virtual void ProcessFormProperties(SPContentType targetContentType, ContentTypeDefinition contentTypeModel)
        {
            if (!string.IsNullOrEmpty(contentTypeModel.NewFormUrl))
                targetContentType.NewFormUrl = contentTypeModel.NewFormUrl;

            if (!string.IsNullOrEmpty(contentTypeModel.NewFormTemplateName))
                targetContentType.NewFormTemplateName = contentTypeModel.NewFormTemplateName;

            if (!string.IsNullOrEmpty(contentTypeModel.EditFormUrl))
                targetContentType.EditFormUrl = contentTypeModel.EditFormUrl;

            if (!string.IsNullOrEmpty(contentTypeModel.EditFormTemplateName))
                targetContentType.EditFormTemplateName = contentTypeModel.EditFormTemplateName;

            if (!string.IsNullOrEmpty(contentTypeModel.DisplayFormUrl))
                targetContentType.DisplayFormUrl = contentTypeModel.DisplayFormUrl;

            if (!string.IsNullOrEmpty(contentTypeModel.DisplayFormTemplateName))
                targetContentType.DisplayFormTemplateName = contentTypeModel.DisplayFormTemplateName;
        }

        protected virtual void ProcessLocalization(SPContentType obj, ContentTypeDefinition definition)
        {
            if (definition.NameResource.Any())
            {
                foreach (var locValue in definition.NameResource)
                    LocalizationService.ProcessUserResource(obj, obj.NameResource, locValue);
            }

            if (definition.DescriptionResource.Any())
            {
                foreach (var locValue in definition.DescriptionResource)
                    LocalizationService.ProcessUserResource(obj, obj.DescriptionResource, locValue);
            }
        }

        #endregion
    }
}
