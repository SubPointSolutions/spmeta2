using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppContentTypeModels
    {
        #region properties

        public static ContentTypeDefinition FoundationAnnouncement = new ContentTypeDefinition
        {
            Id = new Guid("{10E5D203-67FD-4F39-ADD2-7E88ED573948}"),
            Name = "Foundation Announcement",
            Description = "Foundation application announcement",
            Group = ModelConsts.MetadataGroup,
            ParentContentTypeId = SPBuiltInContentTypeId.Announcement.ToString()
        };

        public static ContentTypeDefinition CompanyDepartment = new ContentTypeDefinition
        {
            Id = new Guid("{EBA9C4FC-2BA9-4131-8F79-27E24107EB84}"),
            Name = "Company Department",
            Description = "Out company department",
            Group = ModelConsts.MetadataGroup,
            ParentContentTypeId = SPBuiltInContentTypeId.Item.ToString()
        };

        public static ContentTypeDefinition DepartmentTask = new ContentTypeDefinition
        {
            Id = new Guid("{CB425DCB-F7AB-4706-A94C-0F277060BF14}"),
            Name = "Department task",
            Description = "Task with department scope. Meant to be taken by any free person from the target department.",
            Group = ModelConsts.MetadataGroup,
            ParentContentTypeId = SPBuiltInContentTypeId.Task.ToString()
        };

        #endregion
    }
}
