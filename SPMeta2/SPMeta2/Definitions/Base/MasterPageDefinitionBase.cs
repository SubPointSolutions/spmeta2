using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Base definition for master pages.
    /// </summary>
    /// 
    [Serializable]
    [DataContract]
    public abstract class MasterPageDefinitionBase : PageDefinitionBase
    {
        #region constructors

        protected MasterPageDefinitionBase()
        {
            UIVersion = new List<string>();
            Content = new byte[0];
            NeedOverride = true;

            UIVersion.Add("15");
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string Description { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        public byte[] Content { get; set; }

        [ExpectValidation]
        [ExpectUpdate]
        [DataMember]
        [ExpectNullable]
        public string DefaultCSSFile { get; set; }

        [ExpectValidation]
        [ExpectUpdateAsUIVersion]
        [DataMember]
        public List<string> UIVersion { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("Description", Description)
                          .AddRawPropertyValue("DefaultCSSFile", DefaultCSSFile)
                          .ToString();
        }

        #endregion
    }
}
