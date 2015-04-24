using System;
using Microsoft.SharePoint.Publishing.WebControls;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Webparts
{
    public class ContentBySearchWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentBySearchWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);

            // content editor specific validation

            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var typedModel = model.WithAssertAndCast<ContentBySearchWebPartDefinition>("model", value => value.RequireNotNull());

            var item = host.PageListItem;
            WebPartExtensions.WithExistingWebPart(item, typedModel, (spWebPartManager, spObject) =>
            {
                var typedWebPart = spObject as ContentByQueryWebPart;

                //[FALSE] - [Title]
                //                        [FALSE] - [Id]
                //                        [FALSE] - [ZoneId]
                //                        [FALSE] - [ZoneIndex]
                //                        [FALSE] - [WebpartFileName]
                //                        [FALSE] - [WebpartType]
                //                        [FALSE] - [WebpartXmlTemplate]

                var assert = ServiceFactory.AssertService
                                           .NewAssert(typedModel, typedWebPart)
                                           .ShouldNotBeNull(typedWebPart);

            });
        }

        #endregion
    }
}
