using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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
    public class SiverlightWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SilverlightWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);


            // web specific validation
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SilverlightWebPartDefinition>("model", value => value.RequireNotNull());

            WebPartExtensions.WithExistingWebPart(host.HostFile, definition, (spWebPartManager, spObject) =>
            {
                var web = spWebPartManager.Web;
                var typedObject = spObject as SilverlightWebPart;

                var assert = ServiceFactory.AssertService
                    .NewAssert(definition, typedObject)
                    .ShouldNotBeNull(typedObject);

                if (!string.IsNullOrEmpty(definition.Url))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.Url);
                        var dstProp = d.GetExpressionValue(o => o.Url);

                        var srcUrl = srcProp.Value as string;
                        var dstUrl = dstProp.Value as string;

                        var isValid = false;

                        if (definition.Url.Contains("~sitecollection"))
                        {
                            var siteCollectionUrl = web.Site.ServerRelativeUrl == "/" ? string.Empty : web.Site.ServerRelativeUrl;

                            isValid = srcUrl.Replace("~sitecollection", siteCollectionUrl) == dstUrl;
                        }
                        else if (definition.Url.Contains("~site"))
                        {
                            var siteCollectionUrl = web.ServerRelativeUrl == "/" ? string.Empty : web.ServerRelativeUrl;

                            isValid = srcUrl.Replace("~site", siteCollectionUrl) == dstUrl;
                        }
                        else
                        {
                            isValid = srcUrl == dstUrl;
                        }

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = dstProp,
                            IsValid = isValid
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.Url, "Url is null or empty. Skipping.");
                }

                if (!string.IsNullOrEmpty(definition.CustomInitParameters))
                {
                    assert.ShouldBeEqual(m => m.CustomInitParameters, o => o.CustomInitParameters);
                }
                else
                {
                    assert.SkipProperty(m => m.CustomInitParameters, "CustomInitParameters is null or empty. Skipping.");
                }

            });

            // content editor specific validation
        }

        #endregion
    }
}
