using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Microsoft.SharePoint.Client;

using SPMeta2.Common;
using SPMeta2.CSOM.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.ModelHosts;
using SPMeta2.Services;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

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

        public override void WithResolvingModelHost(ModelHostResolveContext modelHostContext)
        {
            var modelHost = modelHostContext.ModelHost;
            var model = modelHostContext.Model;
            var childModelType = modelHostContext.ChildModelType;
            var action = modelHostContext.Action;

            var site = ExtractSite(modelHost);
            var web = ExtractWeb(modelHost);

            var mdHHost = modelHost as CSOMModelHostBase;

            var contentTypeModel = model as ContentTypeDefinition;

            if (web != null && contentTypeModel != null)
            {
                var context = web.Context;

                if (string.IsNullOrEmpty(contentTypeModel.ParentContentTypeId))
                {
                    var result = context.LoadQuery(web.AvailableContentTypes.Where(ct => ct.Name == contentTypeModel.ParentContentTypeName));
                    context.ExecuteQueryWithTrace();

                    var parentContentType = result.FirstOrDefault();
                    if (parentContentType == null)
                        throw new SPMeta2Exception("Couldn't find parent contenttype with the given name.");

                    //contentTypeModel.ParentContentTypeId = parentContentType.StringId;
                }

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
            if (!currentContentType.IsPropertyAvailable("SchemaXml") || !web.IsPropertyAvailable("ServerRelativeUrl"))
            {
                context.Load(web, w => w.ServerRelativeUrl);
                currentContentType.Context.Load(currentContentType, c => c.SchemaXml);
                currentContentType.Context.ExecuteQueryWithTrace();
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
            var csomModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            var web = ExtractWeb(modelHost);

            var contentTypeModel = model.WithAssertAndCast<ContentTypeDefinition>("model", value => value.RequireNotNull());
            var context = web.Context;

            if (string.IsNullOrEmpty(contentTypeModel.ParentContentTypeId))
            {
                var parentContentTypeName = contentTypeModel.ParentContentTypeName;

                var result = context.LoadQuery(web.AvailableContentTypes.Where(ct => ct.Name == parentContentTypeName));
                context.ExecuteQueryWithTrace();

                var parentContentType = result.FirstOrDefault();
                if (parentContentType == null)
                    throw new SPMeta2Exception("Couldn't find parent contenttype with the given name.");

                // rare case where it's ok to change definition
                contentTypeModel.ParentContentTypeId = parentContentType.Id.ToString();
            }

            var contentTypeName = contentTypeModel.Name;
            var contentTypeId = contentTypeModel.GetContentTypeId();

#if !NET35
            var tmpContentType = context.LoadQuery(web.ContentTypes.Where(ct => ct.StringId == contentTypeId));
            context.ExecuteQueryWithTrace();

            var tmp = tmpContentType.FirstOrDefault();
#endif

#if NET35
            // SP2010 CSOM does not have an option to get the content type by ID
            // fallback to Name, and that's a huge thing all over the M2 library and provision
            
            var tmpContentType = context.LoadQuery(web.ContentTypes.Where(ct => ct.Name == contentTypeName));
            context.ExecuteQueryWithTrace();

            var tmp = tmpContentType.FirstOrDefault();
#endif

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

            ContentType currentContentType;

            if (tmp == null || tmp.ServerObjectIsNull == null || tmp.ServerObjectIsNull.Value)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new content type");

                currentContentType = web.ContentTypes.Add(new ContentTypeCreationInformation
                {
                    Name = contentTypeModel.Name,
                    Description = string.IsNullOrEmpty(contentTypeModel.Description) ? string.Empty : contentTypeModel.Description,
                    Group = contentTypeModel.Group,
#if !NET35
                    Id = contentTypeId,
#endif
                    ParentContentType = null
                });
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing content type");

                currentContentType = tmp;
            }

            context.Load(currentContentType);

#if !NET35
            context.Load(currentContentType, c => c.Sealed);
#endif

            context.ExecuteQueryWithTrace();

#if !NET35
            // CSOM can't update sealed content types
            // adding if-else so that the provision would go further
            if (!currentContentType.Sealed)
            {
#endif
                // doc template first, then set the other props
                // ExtractResourceFolderServerRelativeUrl might make ExecuteQueryWithTrace() call
                // so that might affect setting up other props
                // all props update should go later
                if (!string.IsNullOrEmpty(contentTypeModel.DocumentTemplate))
                {
                    var serverRelativeFolderUrl = ExtractResourceFolderServerRelativeUrl(web, context,
                        currentContentType);

                    var processedDocumentTemplateUrl = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                    {
                        Value = contentTypeModel.DocumentTemplate,
                        Context = csomModelHost
                    }).Value;

                    // resource related path
                    if (!processedDocumentTemplateUrl.Contains('/')
                        && !processedDocumentTemplateUrl.Contains('\\'))
                    {
                        processedDocumentTemplateUrl = UrlUtility.CombineUrl(new[]
                        {
                            serverRelativeFolderUrl,
                            processedDocumentTemplateUrl
                        });
                    }

                    currentContentType.DocumentTemplate = processedDocumentTemplateUrl;
                }

                ProcessFormProperties(currentContentType, contentTypeModel);

                // only after DocumentTemplate processing
                ProcessLocalization(currentContentType, contentTypeModel);
                

                currentContentType.Hidden = contentTypeModel.Hidden;

                currentContentType.Name = contentTypeModel.Name;
                currentContentType.Description = string.IsNullOrEmpty(contentTypeModel.Description)
                    ? string.Empty
                    : contentTypeModel.Description;
                currentContentType.Group = contentTypeModel.Group;

#if !NET35
                if (!string.IsNullOrEmpty(contentTypeModel.JSLink))
                    currentContentType.JSLink = contentTypeModel.JSLink;
#endif

                if (contentTypeModel.ReadOnly.HasValue)
                    currentContentType.ReadOnly = contentTypeModel.ReadOnly.Value;

#if !NET35
                if (contentTypeModel.Sealed.HasValue)
                    currentContentType.Sealed = contentTypeModel.Sealed.Value;

            }

#endif

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

#if !NET35
            if (!currentContentType.Sealed)
            {
                TraceService.Information((int)LogEventId.ModelProvisionCoreCall, "Calling currentContentType.Update(true)");
                currentContentType.Update(true);

                context.ExecuteQueryWithTrace();
            }
#endif
        }

        private void ProcessFormProperties(ContentType targetContentType, ContentTypeDefinition contentTypeModel)
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

            if (string.IsNullOrEmpty(contentTypeModel.ParentContentTypeId))
            {
                var result = context.LoadQuery(rootWeb.AvailableContentTypes.Where(ct => ct.Name == contentTypeModel.ParentContentTypeName));
                context.ExecuteQueryWithTrace();

                var parentContentType = result.FirstOrDefault();
                if (parentContentType == null)
                    throw new SPMeta2Exception("Couldn't find parent contenttype with the given name.");

                // nope, never change the definition props
                //contentTypeModel.ParentContentTypeId = parentContentType.StringId;
            }
            else
            {
                context.ExecuteQueryWithTrace();
            }

            var contentTypeId = contentTypeModel.GetContentTypeId();

#if !NET35
            var currentContentType = contentTypes.FirstOrDefault(c => c.StringId.ToLower() == contentTypeId.ToLower());
#endif

#if NET35
            var currentContentType = contentTypes.FirstOrDefault(c => c.ToString().ToLower() == contentTypeId.ToLower());
#endif

            if (currentContentType != null)
            {
                currentContentType.DeleteObject();
                context.ExecuteQueryWithTrace();
            }
        }

        protected virtual void ProcessLocalization(ContentType obj, ContentTypeDefinition definition)
        {
            ProcessGenericLocalization(obj, new Dictionary<string, List<ValueForUICulture>>
            {
                { "NameResource", definition.NameResource },
                { "DescriptionResource", definition.DescriptionResource },
            });
        }
    }
}
