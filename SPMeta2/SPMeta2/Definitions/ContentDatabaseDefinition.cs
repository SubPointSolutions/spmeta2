using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint content database.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPContentDatabase", "Microsoft.SharePoint")]

    [DefaultParentHostAttribute(typeof(WebApplicationDefinition))]
    [DefaultRootHost(typeof(WebApplicationDefinition))]

    [Serializable] 
    [DataContract]

    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

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
            return new ToStringResult<ContentDatabaseDefinition>(this)
                          .AddPropertyValue(p => p.ServerName)
                          .AddPropertyValue(p => p.DbName)
                          .AddPropertyValue(p => p.WarningSiteCollectionNumber)
                          .AddPropertyValue(p => p.MaximumSiteCollectionNumber)
                          .ToString();
        }

        #endregion
    }
}
