using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
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

        protected override string GetWebpartXmlDefinition(
            ListItemModelHost listItemModelHost,
            WebPartDefinitionBase webPartModel)
        {
            var typedDefinition = webPartModel.WithAssertAndCast<SilverlightWebPartDefinition>("model", value => value.RequireNotNull());
            var wpXml = WebpartXmlExtensions.LoadWebpartXmlDocument(this.ProcessCommonWebpartProperties(BuiltInWebPartTemplates.SilverlightWebPart, webPartModel));

            if (!string.IsNullOrEmpty(typedDefinition.Url))
            {
                var linkValue = typedDefinition.Url;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Original Url: [{0}]", linkValue);

                linkValue = TokenReplacementService.ReplaceTokens(new TokenReplacementContext
                {
                    Value = linkValue,
                    Context = listItemModelHost.HostClientContext
                }).Value;

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Token replaced Url: [{0}]", linkValue);

                wpXml.SetOrUpdateProperty("Url", linkValue);
            }

            if (!string.IsNullOrEmpty(typedDefinition.CustomInitParameters))
            {
                wpXml.SetOrUpdateProperty("CustomInitParameters", typedDefinition.CustomInitParameters);
            }

            return wpXml.ToString();
        }

        #endregion

    }
}
