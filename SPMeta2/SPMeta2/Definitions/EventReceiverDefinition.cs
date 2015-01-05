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
    /// Allows to install SharePoint application to the target web.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPEventReceiverDefinition", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.EventReceiverDefinition", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]

    [Serializable]

    [ExpectWithExtensionMethod]
    public class EventReceiverDefinition : DefinitionBase
    {
        #region constructors

        public EventReceiverDefinition()
        {

        }

        #endregion

        #region properties

        /// <summary>
        /// Target name of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Name { get; set; }

        /// <summary>
        /// Target type of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Type { get; set; }

        /// <summary>
        /// Target assembly of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Assembly { get; set; }

        /// <summary>
        /// Target class of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Class { get; set; }

        /// <summary>
        /// Target sequence number of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Target synchronization of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Synchronization { get; set; }

        /// <summary>
        /// Target data of the event receiver.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Data { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<EventReceiverDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.Type)
                          .AddPropertyValue(p => p.Assembly)
                          .AddPropertyValue(p => p.Class)
                          .AddPropertyValue(p => p.SequenceNumber)
                          .AddPropertyValue(p => p.Synchronization)
                          .ToString();
        }

        #endregion
    }
}
