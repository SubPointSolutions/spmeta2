using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions.Base
{
    [DataContract]
    /// <summary>
    /// Base definitino for web part definitions - generic web part and all other 'typed' web parts.
    /// </summary>
    public abstract class WebPartDefinitionBase : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Title of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        [IdentityKey]
        public string Title { get; set; }

        /// <summary>
        /// ID of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public virtual string Id { get; set; }

        /// <summary>
        /// ZoneId of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        public string ZoneId { get; set; }

        /// <summary>
        /// ZoneIndex of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        public int ZoneIndex { get; set; }

        #endregion

        #region properties

        /// <summary>
        /// File name of the target web part definition from the web part gallery.
        /// 
        /// WebpartFileName is used for the first priority to deploy web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Web part content")]
        [DataMember]
        public string WebpartFileName { get; set; }

        /// <summary>
        /// Type of the target web part.
        /// 
        /// WebpartType is used as a second priority to deploy web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Web part content")]
        [DataMember]
        public string WebpartType { get; set; }

        /// <summary>
        /// XML definition of the target web part.
        /// Both V2 and V3 definition are supported.
        /// 
        /// WebpartXmlTemplate is used as the final step to deploy web part. 
        /// </summary>        
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Web part content")]
        [DataMember]
        public string WebpartXmlTemplate { get; set; }

        /// <summary>
        /// Indicated if the web part should be added to the publishing or wiki page content area.
        /// </summary>
        [DataMember]
        public bool AddToPageContent { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] Id:[{1}] WebpartFileName:[{2}] WebpartType:[{3}] ZoneId:[{4}] ZoneIndex:[{5}]",
                new[] { Title, Id, WebpartFileName, WebpartType, ZoneId, ZoneIndex.ToString() });
        }

        #endregion
    }
}
