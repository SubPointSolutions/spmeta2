using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy alternate URL.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPAlternateUrl", "Microsoft.SharePoint")]

    [DefaultRootHost(typeof(WebApplicationDefinition))]
    [DefaultParentHost(typeof(WebApplicationDefinition))]

    [Serializable]

    [ExpectWithExtensionMethod]
    public class AlternateUrlDefinition : DefinitionBase
    {
        #region properties

        public string Url { get; set; }
        public string UrlZone { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<AlternateUrlDefinition>(this)
                          .AddPropertyValue(p => p.Url)
                          .AddPropertyValue(p => p.UrlZone)

                          .ToString();
        }

        #endregion
    }
}
