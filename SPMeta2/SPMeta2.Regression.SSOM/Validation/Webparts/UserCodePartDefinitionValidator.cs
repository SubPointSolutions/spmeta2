using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Webparts
{
    public class UserCodePartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UserCodeWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);

            // web specific validation
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<UserCodeWebPartDefinition>("model", value => value.RequireNotNull());

            var item = host.PageListItem;

            WebPartExtensions.WithExistingWebPart(item, definition, (spWebPartManager, spObject) =>
            {
                var web = spWebPartManager.Web;
                var typedObject = spObject as SPUserCodeWebPart;

                var assert = ServiceFactory.AssertService
                    .NewAssert(definition, typedObject)
                    .ShouldNotBeNull(typedObject);

            });

            // content editor specific validation
        }

        #endregion
    }
}
