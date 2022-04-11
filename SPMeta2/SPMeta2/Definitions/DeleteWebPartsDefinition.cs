using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFile", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [SingletonIdentity]

    [ParentHostCapability(typeof(WebPartPageDefinition))]
    [ParentHostCapability(typeof(WikiPageDefinition))]

    [ExpectManyInstances]
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

        // ReSharper disable once RedundantOverridenMember
        public override string ToString()
        {
            return new ToStringResultRaw()
                         .ToString();
        }

        #endregion
    }

    [DataContract]
    [Serializable]
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
