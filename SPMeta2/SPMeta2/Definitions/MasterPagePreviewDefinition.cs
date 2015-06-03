using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint asp.net master page.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(ListDefinition), typeof(RootWebDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class MasterPagePreviewDefinition : ContentPageDefinitionBase
    {
        #region constructors

        public MasterPagePreviewDefinition()
        {
            UIVersion = new List<string>();
            NeedOverride = true;

            UIVersion.Add("15");
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectUpdateAsUIVersion]
        [DataMember]
        public List<string> UIVersion { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<MasterPagePreviewDefinition>(this, base.ToString())
                          .ToString();
        }

        #endregion
    }
}
