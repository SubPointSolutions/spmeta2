using System;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Standard.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'Site Feed' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectArrayExtensionMethod]

    public class SiteFeedWebPartDefinition : WebPartDefinition
    {
        #region properties

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SiteFeedWebPartDefinition>(this, base.ToString())
                          .ToString();
        }

        #endregion
    }
}
