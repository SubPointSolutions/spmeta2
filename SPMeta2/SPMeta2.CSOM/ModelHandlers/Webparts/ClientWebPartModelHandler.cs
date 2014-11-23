using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var wpXml = WebpartXmlExtensions
                .LoadWebpartXmlDocument(BuiltInWebPartTemplates.ClientWebPart)
                .SetOrUpdateProperty("FeatureId", wpModel.FeatureId.ToString())
                .SetOrUpdateProperty("ProductId", wpModel.ProductId.ToString())
                .SetOrUpdateProperty("WebPartName", wpModel.WebPartName)
                .SetOrUpdateProperty("ProductWebId", webId)
                .ToString();

            return wpXml;
        }

        #endregion
    }
}
