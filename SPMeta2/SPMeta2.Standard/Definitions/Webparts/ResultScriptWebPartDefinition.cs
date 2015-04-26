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
    /// Allows to define and deploy 'Site Feed' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]
    public class ResultScriptWebPartDefinition : WebPartDefinition
    {
        #region properties

        [DataMember]
        [ExpectValidation]
        public string DataProviderJSON { get; set; }

        [DataMember]
        [ExpectValidation]
        public string EmptyMessage { get; set; }

        [DataMember]
        [ExpectValidation]
        public int? ResultsPerPage { get; set; }

        [DataMember]
        [ExpectValidation]
        public bool? ShowResultCount { get; set; }


        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ResultScriptWebPartDefinition>(this, base.ToString())
                          .ToString();
        }

        #endregion
    }
}
