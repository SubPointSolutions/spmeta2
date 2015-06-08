using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'Silverlight' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]
    public class SilverlightWebPartDefinition : WebPartDefinition
    {
        #region properties

        [DataMember]
        [ExpectValidation]
        public string Url { get; set; }

        [DataMember]
        [ExpectValidation]
        public string CustomInitParameters { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SilverlightWebPartDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.Url)
                          .AddPropertyValue(p => p.CustomInitParameters)
                          .ToString();
        }

        #endregion
    }
}
