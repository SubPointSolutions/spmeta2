using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.Models;
using SPMeta2.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using System.Xml.Linq;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using UrlUtility = SPMeta2.Utils.UrlUtility;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ContentTypeModelHandler : CSOMModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ContentTypeDefinition); }
        }

        protected Site ExtractSite(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite;
            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostSite;

            throw new SPMeta2Exception("modelHost should be SiteModelHost/WebModelHost");
        }

        protected Web ExtractWeb(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb;
            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            throw new SPMeta2Exception("modelHost should be SiteModelHost/WebModelHost");
        }

        public override void WithResolvingModelHost(object modelHost, DefinitionBase model, Type childModelType, Action<object> action)
        {
            var site = ExtractSite(modelHost);
            var web = ExtractWeb(modelHost);

            var mdHHost = modelHost as CSOMModelHostBase;

            var contentTypeModel = model as ContentTypeDefinition;

            if (web != null && contentTypeModel != null)
            {
                var context = web.Context;

                var id = contentTypeModel.GetContentTypeId();
                var currentContentType = web.ContentTypes.GetById(id);

                context.ExecuteQueryWithTrace();

                if (childModelType == typeof(ModuleFileDefinition))
                {
                    TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "Resolving content type resource folder for ModuleFileDefinition");

                    var serverRelativeFolderUrl = ExtractResourceFolderServerRelativeUrl(web, context, currentContentType);

                    var ctFolder = web.GetFolderByServerRelativeUrl(serverRelativeFolderUrl);
                    context.ExecuteQueryWithTrace();

                    var folderModelHost = ModelHostBase.Inherit<FolderModelHost>(mdHHost, host =>
                    {
                        host.CurrentContentType = currentContentType;
                        host.CurrentContentTypeFolder = ctFolder;
                    });

                    action(folderModelHost);
                }
                else
                {
                    action(new ModelHostContext
                    {
                        Site = site,
                        Web = web,
                        ContentType = currentContentType
                    });
                }

                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "Calling currentContentType.Update(true)");
                currentContentType.Update(true);

                context.ExecuteQueryWithTrace();
            }
            else
            {
                action(modelHost);
            }
        }

        private static string ExtractResourceFolderServerRelativeUrl(Web web, ClientRuntimeContext context, ContentType currentContentType)
        {
            if (!currentContentType.IsPropertyAvailable("SchemaXml")
                || !web.IsPropertyAvailable("ServerRelativeUrl"))
            {
                context.Load(web, w => w.ServerRelativeUrl);
                currentContentType.Context.Load(currentContentType, c => c.SchemaXml);
                currentContentType.Context.ExecuteQuery();
            }

            var ctDocument = XDocument.Parse(currentContentType.SchemaXml);
            var folderUrlNode = ctDocument.Descendants().FirstOrDefault(d => d.Name == "Folder");

            var webRelativeFolderUrl = folderUrlNode.Attribute("TargetName").Value;
            var serverRelativeFolderUrl = UrlUtility.CombineUrl(web.ServerRelativeUrl, webRelativeFolderUrl);

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "webRelativeFolderUrl is: [{0}]", webRelativeFolderUrl);
            return serverRelativeFolderUrl;
        }



        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var site = ExtractSite(modelHost);
            var web = ExtractWeb(modelHost);

            var contentTypeModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());
            var context = web.Context;

            var contentTypeId = contentTypeModel.GetContentTypeId();

            var tmpContentType = context.LoadQuery(web.ContentTypes.Where(ct => ct.StringId == contentTypeId));
            context.ExecuteQuery();

            var tmp = tmpContentType.FirstOrDefault();

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = tmp,
                ObjectType = typeof(ContentType),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            InvokeOnModelEvent<ContentTypeDefinition, ContentType>(null, ModelEventType.OnUpdating);

            ContentType currentContentType = null;

            if (tmp == null || tmp.ServerObjectIsNull == null || tmp.ServerObjectIsNull.Value)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new content type");

                currentContentType = web.ContentTypes.Add(new ContentTypeCreationInformation
                {
                    Name = contentTypeModel.Name,
                    Description = string.IsNullOrEmpty(contentTypeModel.Description) ? string.Empty : contentTypeModel.Description,
                    Group = contentTypeModel.Group,
                    Id = contentTypeId,
                    ParentContentType = null
                });
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing content type");

                currentContentType = tmp;
            }

            currentContentType.Hidden = contentTypeModel.Hidden;

            currentContentType.Name = contentTypeModel.Name;
            currentContentType.Description = string.IsNullOrEmpty(contentTypeModel.Description) ? string.Empty : contentTypeModel.Description;
            currentContentType.Group = contentTypeModel.Group;

            if (!string.IsNullOrEmpty(contentTypeModel.DocumentTemplate))
            {
                var serverRelativeFolderUrl = ExtractResourceFolderServerRelativeUrl(web, context, currentContentType);

                var processedDocumentTemplateUrl = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = contentTypeModel.DocumentTemplate,
                    Context = context
                }).Value;

                // resource related path
                if (!processedDocumentTemplateUrl.Contains('/')
                    && !processedDocumentTemplateUrl.Contains('\\'))
                {
                    processedDocumentTemplateUrl = UrlUtility.CombineUrl(new string[] { 
                            serverRelativeFolderUrl,
                            processedDocumentTemplateUrl
                        });
                }

                currentContentType.DocumentTemplate = processedDocumentTemplateUrl;
            }

            InvokeOnModelEvent<ContentTypeDefinition, ContentType>(currentContentType, ModelEventType.OnUpdated);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentContentType,
                ObjectType = typeof(ContentType),
                ObjectDefinition = model,
                ModelHost = modelHost
            });

            TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "Calling currentContentType.Update(true)");
            currentContentType.Update(true);

            context.ExecuteQueryWithTrace();
        }

        public override void RetractModel(object modelHost, DefinitionBase model)
        {
            var siteModelHost = modelHost.WithAssertAndCast<SiteModelHost>("modelHost", value => value.RequireNotNull());

            var site = siteModelHost.HostSite;
            var contentTypeModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());

            var context = site.Context;
            var rootWeb = site.RootWeb;

            var contentTypes = rootWeb.ContentTypes;

            context.Load(rootWeb);
            context.Load(contentTypes);

            context.ExecuteQueryWithTrace();

            var contentTypeId = contentTypeModel.GetContentTypeId();

            var currentContentType = contentTypes.FirstOrDefault(c => c.StringId.ToLower() == contentTypeId.ToLower());

            if (currentContentType != null)
            {
                currentContentType.DeleteObject();
                context.ExecuteQueryWithTrace();
            }
        }
    }

}
