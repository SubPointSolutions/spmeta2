using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Attributes.Identity
{
    public class IdentityKeyAttribute : Attribute
    {
        #region properties

        public string GroupName { get; set; }

        #endregion
    }
}
