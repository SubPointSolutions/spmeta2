using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.SSOM.ModelHandlers.Webparts
{
    public class SilverlightWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(SilverlightWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<SilverlightWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(SilverlightWebPart).AssemblyQualifiedName;

        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<SilverlightWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedDefinition = webpartModel.WithAssertAndCast<SilverlightWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(typedDefinition.Url))
            {
                var linkValue = typedDefinition.Url;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original Url: [{0}]", linkValue);


                linkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = linkValue,
                    Context = CurrentHost.PageListItem.Web
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced Url: [{0}]", linkValue);

                typedWebpart.Url = linkValue;
            }

            if (!string.IsNullOrEmpty(typedDefinition.CustomInitParameters))
            {
                typedWebpart.CustomInitParameters = typedDefinition.CustomInitParameters;
            }
        }

        #endregion
    }
}
