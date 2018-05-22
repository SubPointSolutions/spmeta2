using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

namespace SPMeta2.Standard.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint managed property.
    /// </summary>
    /// 

    [DataContract]
    public class ManagedPropertyMappping
    {
        #region properties

        [DataMember]
        public string CrawledPropertyName { get; set; }

        #endregion
    }

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.Server.Search.Administration.ManagedProperty", "Microsoft.Office.Server.Search")]
    //[SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(FarmDefinition))]
    [DefaultParentHost(typeof(FarmDefinition))]

    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(FarmDefinition))]

    [ExpectManyInstances]

    public class ManagedPropertyDefinition : DefinitionBase
    {
        #region constructors

        public ManagedPropertyDefinition()
        {
            Mappings = new List<ManagedPropertyMappping>();
        }

        #endregion

        #region properties

        [DataMember]
        public List<ManagedPropertyMappping> Mappings { get; set; }

        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ManagedType { get; set; }

        [DataMember]
        public bool? Searchable { get; set; }

        [DataMember]
        public bool? Queryable { get; set; }

        [DataMember]
        public bool? Retrievable { get; set; }

        [DataMember]
        public bool? HasMultipleValues { get; set; }

        [DataMember]
        public bool? Refinable { get; set; }

        [DataMember]
        public bool? Sortable { get; set; }

        [DataMember]
        public bool? SafeForAnonymous { get; set; }

        //public string Alias { get; set; }

        [DataMember]
        public bool? TokenNormalization { get; set; }

        //public bool? CompleteMatching { get; set; }

        // public bool? CompanyExtraction { get; set; }

        #endregion

        #region methods


        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("Description", Description)
                          .ToString();
        }

        #endregion


    }
}
