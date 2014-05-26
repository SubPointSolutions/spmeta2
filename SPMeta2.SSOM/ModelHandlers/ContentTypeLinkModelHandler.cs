using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ContentTypeLinkModelHandler : ModelHandlerBase
    {
        #region methods

        public override Type TargetType
        {
            get { return typeof(ContentTypeLinkDefinition); }
        }

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var list = modelHost.WithAssertAndCast<SPList>("modelHost", value => value.RequireNotNull());
            var contentTypeLinkModel = model.WithAssertAndCast<ContentTypeLinkDefinition>("model", value => value.RequireNotNull());

            if (list.ContentTypesEnabled)
            {
                var rootWeb = list.ParentWeb.Site.RootWeb;

                var contentTypeId = new SPContentTypeId(contentTypeLinkModel.ContentTypeId);
                var targetContentType = rootWeb.ContentTypes[contentTypeId];

                if (targetContentType != null && list.ContentTypes[targetContentType.Name] == null)
                {
                    list.ContentTypes.Add(targetContentType);
                    list.Update();
                }
            }
        }

        #endregion
    }
}
