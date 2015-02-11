using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHandlers.Webparts;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Webparts
{
    public class ClientXsltListViewWebPartDefinitionValidator : WebPartModelHandler
    {
        public override Type TargetType
        {
            get { return typeof(XsltListViewWebPartDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<XsltListViewWebPartDefinition>("model", value => value.RequireNotNull());

            var web = listItemModelHost.HostWeb;
            var context = web.Context;

            var pageItem = listItemModelHost.HostListItem;

            WithWithExistingWebPart(pageItem, definition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, definition, spObject)
                                                 .ShouldNotBeNull(spObject);

                var skipMessage = "Property is not available in CSOM. Skipping.";

                // some of the properties can actually be validated
                // http://stackoverflow.com/questions/11814829/how-to-read-webpart-content-using-sharepoint-client-om
                // asmx calls are required to get additional information about the current web parts

                assert
                    .SkipProperty(m => m.ZoneIndex, skipMessage)

                    .SkipProperty(m => m.Id, skipMessage)
                    .SkipProperty(m => m.ZoneId, skipMessage)

                    .SkipProperty(m => m.WebpartFileName, skipMessage)
                    .SkipProperty(m => m.WebpartType, skipMessage)
                    .SkipProperty(m => m.WebpartXmlTemplate, skipMessage)

                    .ShouldBeEqual(m => m.Title, o => o.Title);

                // typed validation
                // we can't really get WP properties yet (XML export will be done later)
                // so skip yet

                assert
                    .SkipProperty(m => m.JSLink, skipMessage)

                    .SkipProperty(m => m.ListId, skipMessage)
                    .SkipProperty(m => m.ListTitle, skipMessage)
                    .SkipProperty(m => m.ListUrl, skipMessage)

                    .SkipProperty(m => m.ViewId, skipMessage)
                    .SkipProperty(m => m.ViewName, skipMessage)

                    .SkipProperty(m => m.TitleUrl, skipMessage);
            });
        }
    }
}
