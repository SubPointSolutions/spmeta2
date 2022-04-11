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
    /// Allows to install SharePoint application to the target web.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPEventReceiverDefinition", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.EventReceiverDefinition", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]
    [DataContract]

    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]
    [ParentHostCapability(typeof(WebDefinition))]
    [ParentHostCapability(typeof(ListDefinition))]
    [ParentHostCapability(typeof(ContentTypeDefinition))]

    [ExpectManyInstances]
    public class EventReceiverDefinition : DefinitionBase
    {
        #region constructors

        #endregion

        #region properties

        /// <summary>
        /// Target name of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        [IdentityKey]
        public string Name { get; set; }

        /// <summary>
        /// Target type of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Target assembly of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string Assembly { get; set; }

        /// <summary>
        /// Target class of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string Class { get; set; }

        /// <summary>
        /// Target sequence number of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Target synchronization of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string Synchronization { get; set; }

        /// <summary>
        /// Target data of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string Data { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Name", Name)
                          .AddRawPropertyValue("Type", Type)
                          .AddRawPropertyValue("Assembly", Assembly)
                          .AddRawPropertyValue("Class", Class)
                          .AddRawPropertyValue("SequenceNumber", SequenceNumber)
                          .AddRawPropertyValue("Synchronization", Synchronization)
                          .ToString();
        }

        #endregion
    }
}
