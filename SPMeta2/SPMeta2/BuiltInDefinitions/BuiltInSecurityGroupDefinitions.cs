using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.BuiltInDefinitions
{
    /// <summary>
    /// Out of the box SharePoint security groups.
    /// </summary>
    public static class BuiltInSecurityGroupDefinitions
    {
        #region properties

        public static SecurityGroupDefinition AssociatedMemberGroup = new SecurityGroupDefinition
        {
            IsAssociatedMemberGroup = true
        };

        public static SecurityGroupDefinition AssociatedOwnerGroup = new SecurityGroupDefinition
        {
            IsAssociatedOwnerGroup = true
        };

        public static SecurityGroupDefinition AssociatedVisitorsGroup = new SecurityGroupDefinition
        {
            IsAssociatedVisitorsGroup = true
        };

        #endregion
    }
}
