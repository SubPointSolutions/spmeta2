using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint content database.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPContentDatabase", "Microsoft.SharePoint")]

    [DefaultParentHost(typeof(WebApplicationDefinition))]
    [DefaultRootHost(typeof(WebApplicationDefinition))]

    [Serializable] 
    [DataContract]

    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(WebApplicationDefinition))]

    [ExpectManyInstances]
    public class ContentDatabaseDefinition : DefinitionBase
    {
        #region constructors

        public ContentDatabaseDefinition()
        {
            Status = 0;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        [ExpectRequired]
        public string ServerName { get; set; }

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string DbName { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string UserPassword { get; set; }

        [ExpectValidation]
        [DataMember]
        public int WarningSiteCollectionNumber { get; set; }

        [ExpectValidation]
        [DataMember]
        public int MaximumSiteCollectionNumber { get; set; }

        [DataMember]
        public int Status { get; set; }

        #endregion

        #region methods
      
        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("ServerName", ServerName)
                          .AddRawPropertyValue("DbName", DbName)
                          .AddRawPropertyValue("WarningSiteCollectionNumber", WarningSiteCollectionNumber)
                          .AddRawPropertyValue("MaximumSiteCollectionNumber", MaximumSiteCollectionNumber)
                          .ToString();
        }

        #endregion
    }
}
