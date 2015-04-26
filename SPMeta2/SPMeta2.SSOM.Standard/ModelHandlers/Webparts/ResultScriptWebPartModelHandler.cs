using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.UI.WebControls.WebParts;
using Microsoft.Office.Server.Search.WebControls;
using Microsoft.SharePoint.Portal.WebControls;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Webparts
{
    public class ResultScriptWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ResultScriptWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<ResultScriptWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(ResultScriptWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<ResultScriptWebPart>("webpartInstance", value => value.RequireNotNull());
            var definition = webpartModel.WithAssertAndCast<ResultScriptWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            if (!string.IsNullOrEmpty(definition.DataProviderJSON))
                typedWebpart.DataProviderJSON = definition.DataProviderJSON;

            if (!string.IsNullOrEmpty(definition.EmptyMessage))
                typedWebpart.EmptyMessage = definition.EmptyMessage;

            if (definition.ResultsPerPage.HasValue)
                typedWebpart.ResultsPerPage = definition.ResultsPerPage.Value;

            if (definition.ShowResultCount.HasValue)
                typedWebpart.ShowResultCount = definition.ShowResultCount.Value;
        }

        #endregion
    }
}
