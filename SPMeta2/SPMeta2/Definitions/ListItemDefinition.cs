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
    /// <summary>
    /// Allows to define and deploy list item to the target list.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPListItem", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ListItem", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(ListDefinition))]

    [ExpectManyInstances]
    public class ListItemDefinition : DefinitionBase
    {
        #region constructors

        public ListItemDefinition()
        {
            Overwrite = true;
            //Content = new byte[0];

            DefaultValues = new List<FieldValue>();
            Values = new List<FieldValue>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Title of the target list item.
        /// </summary>
        /// 

        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Title { get; set; }

        /// <summary>
        /// Should item be overwritten.
        /// </summary>
        /// 

        [DataMember]
        public bool Overwrite { get; set; }

        /// <summary>
        /// Should SystemUpdate() be used.
        /// </summary>
        /// 
        [DataMember]
        public bool SystemUpdate { get; set; }

        /// <summary>
        /// Should SystemUpdateIncrementVersionNumber be used.
        /// </summary>
        /// 
        [DataMember]
        public bool SystemUpdateIncrementVersionNumber { get; set; }

        /// <summary>
        /// Should UpdateOverwriteVersion be used.
        /// </summary>
        /// 
        [DataMember]
        public bool UpdateOverwriteVersion { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string ContentTypeId { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string ContentTypeName { get; set; }

        [ExpectValidation]
        [DataMember]
        public List<FieldValue> DefaultValues { get; set; }


        [ExpectValidation]
        [DataMember]
        public List<FieldValue> Values { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Title", Title)
                          .AddRawPropertyValue("Overwrite", Overwrite)
                          .AddRawPropertyValue("SystemUpdate", SystemUpdate)
                          .AddRawPropertyValue("SystemUpdateIncrementVersionNumber", SystemUpdateIncrementVersionNumber)
                          .AddRawPropertyValue("UpdateOverwriteVersion", UpdateOverwriteVersion)
                          .ToString();
        }

        #endregion
    }
}
