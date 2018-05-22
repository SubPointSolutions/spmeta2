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
    /// Reserved for the future usage.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPFarm", "Microsoft.SharePoint")]
    [Serializable]
    [DataContract]
    [ExpectAddHostExtensionMethod]
    [SingletonIdentity]

    [ParentHostCapability(typeof(FarmDefinition), IsRoot = true)]
    public class FarmDefinition : DefinitionBase
    {
        #region properties

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .ToString();
        }

        #endregion
    }
}
