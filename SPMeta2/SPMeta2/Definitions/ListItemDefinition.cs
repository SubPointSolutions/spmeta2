using SPMeta2.Attributes;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy list item to the target list.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPListItem", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ListItem", "Microsoft.SharePoint.Client")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(ListDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class ListItemDefinition : DefinitionBase
    {
        #region constructors

        public ListItemDefinition()
        {
            Overwrite = true;
            //Content = new byte[0];
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

        // should be collection of attachments later
        //public byte[] Content { get; set; }

        // TODO, serializable dictionary for propertied such as content type and so on

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ListItemDefinition>(this)
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.Overwrite)
                          .AddPropertyValue(p => p.SystemUpdate)
                          .AddPropertyValue(p => p.SystemUpdateIncrementVersionNumber)
                          .AddPropertyValue(p => p.UpdateOverwriteVersion)
                          .ToString();
        }

        #endregion
    }
}
