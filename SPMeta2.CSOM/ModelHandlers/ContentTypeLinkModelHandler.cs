using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class ContentTypeLinkModelHandler : ModelHandlerBase
    {
        public override Type TargetType
        {
            get { return typeof(ContentTypeLinkDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<List>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            var context = list.Context;

            context.Load(list, l => l.ContentTypesEnabled);
            context.ExecuteQuery();

            if (list.ContentTypesEnabled)
            {
                var web = list.ParentWeb;

                context.Load(web, w => w.AvailableContentTypes);
                context.Load(list, l => l.ContentTypes);

                context.ExecuteQuery();

                var targetContentType = FindSiteContentType(web, contentTypeLinkModel);
                var listContentType = FindListContentType(list, contentTypeLinkModel);

                if (targetContentType != null && listContentType == null)
                {
                    list.ContentTypes.Add(new ContentTypeCreationInformation
                    {
                        Description = targetContentType.Description,
                        Group = targetContentType.Group,
                        Name = targetContentType.Name,
                        ParentContentType = targetContentType
                    });

                    list.Update();
                    context.ExecuteQuery();
                }
            }
        }

        protected ContentType FindListContentType(List list, ContentTypeLinkDefinition contentTypeLinkModel)
        {
            return list.ContentTypes.FindByName(contentTypeLinkModel.ContentTypeName);
        }

        protected ContentType FindSiteContentType(Web web, ContentTypeLinkDefinition contentTypeLinkModel)
        {
            ContentType targetContentType = null;

            if (!string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeName))
                targetContentType = web.AvailableContentTypes.FindByName(contentTypeLinkModel.ContentTypeName);

            if (targetContentType == null && !string.IsNullOrEmpty(contentTypeLinkModel.ContentTypeId))
                targetContentType = web.AvailableContentTypes.FindById(contentTypeLinkModel.ContentTypeId);

            if (targetContentType == null)
                throw new Exception(string.Format("Cannot find content type specified by model: id:[{0}] name:[{1}]",
                                            contentTypeLinkModel.ContentTypeId, contentTypeLinkModel.ContentTypeName));

            return targetContentType;
        }
    }
}
