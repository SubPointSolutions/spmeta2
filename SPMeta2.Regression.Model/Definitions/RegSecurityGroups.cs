using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegSecurityGroups
    {
        #region properties

        public static SecurityGroupDefinition SecurityGroup1 = new SecurityGroupDefinition
        {
            Name = "Security Group 1",
            Description = "Group 1.",
        };

        public static SecurityGroupDefinition SecurityGroup2 = new SecurityGroupDefinition
        {
            Name = "Security Group 2",
            Description = "Group 2.",
        };

        public static SecurityGroupDefinition SecurityGroup3 = new SecurityGroupDefinition
        {
            Name = "Security Group 3",
            Description = "Group 3.",
        };

        #endregion
    }
}
