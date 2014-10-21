using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHandlers.Webparts;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Webparts
{
    public class SiteFeedWebPartDefinitionValidator : WebPartModelHandler
    {
        public override Type TargetType
        {
            get { return typeof(SiteFeedWebPartDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SiteFeedWebPartDefinition>("model", value => value.RequireNotNull());

            var pageItem = listItemModelHost.HostListItem;

            WithWithExistingWebPart(pageItem, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

                // some of the properties can actually be validated
                // http://stackoverflow.com/questions/11814829/how-to-read-webpart-content-using-sharepoint-client-om
                // asmx calls are required to get additional information about the current web parts

                assert
                    .SkipProperty(m => m.ZoneIndex, "Property is not available in CSOM. Skipping.")

                    .SkipProperty(m => m.Id, "Property is not available in CSOM. Skipping.")
                    .SkipProperty(m => m.ZoneId, "Property is not available in CSOM. Skipping.")

                    .SkipProperty(m => m.WebpartFileName, "Property is not available in CSOM. Skipping.")
                    .SkipProperty(m => m.WebpartType, "Property is not available in CSOM. Skipping.")
                    .SkipProperty(m => m.WebpartXmlTemplate, "Property is not available in CSOM. Skipping.")

                    .ShouldBeEqual(m => m.Title, o => o.Title);
            });
        }
    }
}
