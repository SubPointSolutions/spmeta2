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
    /// Allows to define and deploy discussion item reply to the target discussion list.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPListItem", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.ListItem", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(DiscussionItemDefinition))]

    //[ExpectAddHostExtensionMethod]
    [Serializable]
    [DataContract]
    //[ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(DiscussionItemDefinition))]

    [ExpectManyInstances]
    public class DiscussionReplyItemDefinition : ListItemDefinition
    {
        #region constructors

        public DiscussionReplyItemDefinition()
        {

        }

        #endregion

        #region properties

        /// <summary>
        /// Body of the discussion reply.
        /// </summary>
        [ExpectValidation]
        [ExpectRequired]
        [DataMember]
        public string Body { get; set; }


        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("Body", Body)
                          .ToString();
        }

        #endregion
    }
}
