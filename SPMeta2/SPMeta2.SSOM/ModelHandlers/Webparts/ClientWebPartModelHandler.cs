﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.SSOM.ModelHandlers.Webparts
{
    public class ClientWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ClientWebPartDefinition); }
        }

        #endregion

        #region methods

        private WebpartPageModelHost _host;

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            _host = host;

            var typedModel = webpartModel.WithAssertAndCast<ClientWebPartDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(ClientWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            base.ProcessWebpartProperties(webpartInstance, webpartModel);

            var typedWebpart = webpartInstance.WithAssertAndCast<ClientWebPart>("webpartInstance", value => value.RequireNotNull());
            var typedModel = webpartModel.WithAssertAndCast<ClientWebPartDefinition>("webpartModel", value => value.RequireNotNull());

            // Enhance 'ClientWebPart' provision - ProductWebId should be current web by default #623
            // https://github.com/SubPointSolutions/spmeta2/issues/623
            var productId = typedModel.ProductId;

            if (!productId.HasGuidValue())
                productId = _host.SPLimitedWebPartManager.Web.ID;

            typedWebpart.FeatureId = typedModel.FeatureId;
            typedWebpart.ProductId = productId.Value;
            typedWebpart.WebPartName = typedModel.WebPartName;
            typedWebpart.ProductWebId = _host.SPLimitedWebPartManager.Web.ID;
        }

        #endregion
    }
}
