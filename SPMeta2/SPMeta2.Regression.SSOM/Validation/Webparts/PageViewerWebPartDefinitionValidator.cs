using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Webparts
{
    public class PageViewerWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(PageViewerWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);

            // web specific validation
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<PageViewerWebPartDefinition>("model", value => value.RequireNotNull());

            var item = host.PageListItem;

            WebPartExtensions.WithExistingWebPart(item, definition, (spWebPartManager, spObject) =>
            {
                var web = spWebPartManager.Web;
                var typedObject = spObject as PageViewerWebPart;

                var assert = ServiceFactory.AssertService
                    .NewAssert(definition, typedObject)
                    .ShouldNotBeNull(typedObject);

                if (!string.IsNullOrEmpty(definition.ContentLink))
                    assert.ShouldBeEqual(m => m.ContentLink, o => o.ContentLink);
                else
                    assert.SkipProperty(m => m.ContentLink);

                if (!string.IsNullOrEmpty(definition.SourceType))
                    assert.ShouldBeEqual(m => m.SourceType, o => o.GetSourceType());
                else
                    assert.SkipProperty(m => m.SourceType);
            });
        }

        #endregion
    }

    internal static class PageViewerWebPartExtensions
    {
        public static string GetSourceType(this PageViewerWebPart wp)
        {
            return wp.SourceType.ToString();
        }
    }
}
