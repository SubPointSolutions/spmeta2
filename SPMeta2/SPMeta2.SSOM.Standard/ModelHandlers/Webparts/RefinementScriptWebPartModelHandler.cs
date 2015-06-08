using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.UI.WebControls.WebParts;
using Microsoft.Office.Server.Search.WebControls;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Webparts
{
    public class RefinementScriptWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(RefinementScriptWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<RefinementScriptWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(RefinementScriptWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition definition)
        {
            base.ProcessWebpartProperties(webpartInstance, definition);

            var typedWebpart = webpartInstance.WithAssertAndCast<RefinementScriptWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedModel = definition.WithAssertAndCast<RefinementScriptWebPartDefinition>("webpartModel", value => value.RequireNotNull());


            if (!string.IsNullOrEmpty(typedModel.SelectedRefinementControlsJson))
                typedWebpart.SelectedRefinementControlsJson = typedModel.SelectedRefinementControlsJson;

            if (!string.IsNullOrEmpty(typedModel.EmptyMessage))
                typedWebpart.EmptyMessage = typedModel.EmptyMessage;
        }

        #endregion
    }
}
