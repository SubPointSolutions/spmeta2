using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'RefinementScriptWebPart' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]
    public class RefinementScriptWebPartDefinition : WebPartDefinition
    {
        #region properties

        [DataMember]
        public string SelectedRefinementControlsJson { get; set; }


        [DataMember]
        public string EmptyMessage { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<RefinementScriptWebPartDefinition>(this, base.ToString())
                          .ToString();
        }

        #endregion
    }
}
