using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFolder", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Folder", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebDefinition))]

    [Serializable]
    [DataContract]
    public class WelcomePageDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// A web relative URL to the target page.
        /// </summary>

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Url { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<WelcomePageDefinition>(this)
                          .AddPropertyValue(p => p.Url)

                          .ToString();
        }

        #endregion
    }
}
