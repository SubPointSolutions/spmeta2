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
            var webPartDefenitions = webpartOnPage.OfType<WebPart>();

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                 .ShouldNotBeNull(webpartOnPage);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.WebParts);
                var webPartMatches = s.WebParts;

                var isValid = true;

                if (webPartMatches.Count == 0)
                {
                    isValid = webPartDefenitions.Count() == 0;
                }
                else
                {
                    // title only yet
                    // should not be any by mentioned title
                    var wpTitles = webPartMatches.Select(wpMatch => wpMatch.Title);
                    isValid = webPartDefenitions.Count(w => wpTitles.Contains(w.Title)) == 0;
                }

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
