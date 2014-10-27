using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'apps part' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    public class ClientWebPartDefinition : WebPartDefinition
    {
        #region properties

        public Guid FeatureId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductWebId { get; set; }

        public string WebPartName { get; set; }

        #endregion


        #region methods

        public override string ToString()
        {
            return new ToStringResult<ClientWebPartDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.FeatureId)
                          .AddPropertyValue(p => p.ProductId)
                          .AddPropertyValue(p => p.ProductWebId)
                          .AddPropertyValue(p => p.WebPartName)
                          .ToString();
        }

        #endregion
    }
}
