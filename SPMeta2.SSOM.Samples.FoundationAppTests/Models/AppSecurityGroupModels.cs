using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppSecurityGroupModels
    {
        #region properties

        public static SecurityGroupDefinition Contractors = new SecurityGroupDefinition
        {
            Name = "Contractors",
            Description = "Group for contractors management.",
        };

        public static SecurityGroupDefinition Students = new SecurityGroupDefinition
        {
            Name = "Students",
            Description = "Group for students management.",
        };

        public static SecurityGroupDefinition AnnounsmentsEditors = new SecurityGroupDefinition
        {
            Name = "Announcements Editors",
            Description = "Group for managing announcements editors.",
        };

        #endregion
    }
}
