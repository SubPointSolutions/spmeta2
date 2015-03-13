using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Utils;

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
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class MasterPageDefinition : PageDefinitionBase
    {
        #region constructors

        public MasterPageDefinition()
        {
            UIVersion = new List<string>();
            Content = new byte[0];
            NeedOverride = true;

            UIVersion.Add("15");
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectUpdate]
        public string Description { get; set; }

        public byte[] Content { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        public string DefaultCSSFile { get; set; }

        [ExpectValidation]
        [ExpectUpdateAsUIVersion]
        public List<string> UIVersion { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<MasterPageDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.Description)
                          .AddPropertyValue(p => p.DefaultCSSFile)
                          .ToString();
        }

        #endregion
    }
}
