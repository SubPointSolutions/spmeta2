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
    /// Allows to define and deploy SharePoint managed account.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPManagedAccount", "Microsoft.SharePoint")]

    [DefaultRootHostAttribute(typeof(FarmDefinition))]
    [DefaultParentHostAttribute(typeof(FarmDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]
    public class ManagedAccountDefinition : DefinitionBase
    {
        #region properties

        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public string LoginName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ManagedAccountDefinition>(this)
                          .AddPropertyValue(p => p.LoginName)
                          .ToString();
        }

        #endregion
    }
}
