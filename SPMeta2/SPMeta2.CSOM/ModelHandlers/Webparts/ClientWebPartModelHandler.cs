using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class ClientWebPartModelHandler : WebPartModelHandler
    {
        public ClientWebPartModelHandler()
        {
            ShouldUseWebPartStoreKeyForWikiPage = true;
        }

        #region properties

        public override Type TargetType
        {
            get { return typeof(ClientWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            if (!listItemModelHost.HostWeb.IsObjectPropertyInstantiated("Id"))
            {
                var webContext = listItemModelHost.HostWeb.Context;
                webContext.Load(listItemModelHost.HostWeb, w => w.Id);
                webContext.ExecuteQueryWithTrace();
            }

            var webId = listItemModelHost.HostWeb.Id.ToString();
            var wpModel = webPartModel.WithAssertAndCast<ClientWebPartDefinition>("model", value => value.RequireNotNull());

            // Enhance 'ClientWebPart' provision - ProductWebId should be current web by default #623
            // https://github.com/SubPointSolutions/spmeta2/issues/623
            var productId = wpModel.ProductId;

            if (!productId.HasGuidValue())
                productId = listItemModelHost.HostWeb.Id;

            var wpXml = WebpartXmlExtensions
                .LoadWebpartXmlDocument(BuiltInWebPartTemplates.ClientWebPart)
                .SetOrUpdateProperty("FeatureId", wpModel.FeatureId.ToString())
                .SetOrUpdateProperty("ProductId", productId.ToString())
                .SetOrUpdateProperty("WebPartName", wpModel.WebPartName)
                .SetOrUpdateProperty("ProductWebId", webId)
                .ToString();

            return wpXml;
        }

        protected override string ProcessCommonWebpartProperties(string webPartXml, WebPartDefinitionBase definition)
        {
            var result = base.ProcessCommonWebpartProperties(webPartXml, definition);

            var wpXml = WebpartXmlExtensions
                .LoadWebpartXmlDocument(result)
                // Error while putting ClientWebPart to a WikiPage #575
                .RemoveProperty("Id")
                .ToString();

            return wpXml;
        }

        #endregion
    }
}
