using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SPMeta2.Utils;
using SPMeta2.SSOM.Extensions;
using System.Web.UI.WebControls.WebParts;
using SPMeta2.Exceptions;

using Microsoft.SharePoint;
using System.IO;
using System.Xml;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class DeleteWebPartsDefinitionValidator : DeleteWebPartsModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<DeleteWebPartsDefinition>("model", value => value.RequireNotNull());

            var spObject = host.HostFile;
            var webpartOnPage = host.SPLimitedWebPartManager.WebParts;

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                 .ShouldNotBeNull(webpartOnPage);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.WebParts);
                var isValid = webpartOnPage.Count == 0;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = null,
                    IsValid = isValid
                };
            });
        }
    }


}
