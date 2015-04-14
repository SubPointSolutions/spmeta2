using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [SingletonIdentity]
    public class DeleteWebPartsDefinition : DefinitionBase
    {
        #region constructors

        public DeleteWebPartsDefinition()
        {
            WebParts = new List<WebPartMatch>();
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        public List<WebPartMatch> WebParts { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }

    [DataContract]
    public class WebPartMatch
    {
        #region properties

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string WebpartType { get; set; }

        #endregion
    }
}
