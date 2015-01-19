using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions
{
    /// <summary>
    /// Allows to define and deploy custom document id provider.
    /// </summary>
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPSite", "Microsoft.SharePoint")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    public class CustomDocumentIdProviderDefinition : DefinitionBase
    {
        #region properties

        public string DocumentProviderType { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<CustomDocumentIdProviderDefinition>(this)
                          .AddPropertyValue(p => p.DocumentProviderType)
                          .ToString();
        }

        #endregion
    }
}
