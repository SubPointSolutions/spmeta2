using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Attributes;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Reserved for the future usage.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPFarm", "Microsoft.SharePoint")]
    [Serializable]
    public class FarmDefinition : DefinitionBase
    {
        #region properties

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<FarmDefinition>(this)
                          .ToString();
        }

        #endregion
    }
}
