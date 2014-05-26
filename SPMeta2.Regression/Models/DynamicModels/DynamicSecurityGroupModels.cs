using System;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Models.DynamicModels
{
    public static class DynamicSecurityGroupModels
    {
        public static SecurityGroupDefinition Contractors = GetSecurityGroupTestTemplate("Contractors");
        public static SecurityGroupDefinition Workers = GetSecurityGroupTestTemplate("Workers");
        public static SecurityGroupDefinition Students = GetSecurityGroupTestTemplate("Students");

        #region methods

        public static SecurityGroupDefinition GetSecurityGroupTestTemplate(string groupPrefix)
        {
            return new SecurityGroupDefinition
            {
                Name = string.Format("{0}{1}", groupPrefix, Environment.TickCount),
                Description = string.Format("{0}description{1}", groupPrefix, Environment.TickCount)
            };
        }

        #endregion
    }

}
